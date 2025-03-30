// <copyright file="EncryptExtension.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using WALLET_SERVICE.Application.Common.Exceptions;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Infrastructure.Common.Static;

namespace WALLET_SERVICE.Infrastructure.Extensions
{
	[ExcludeFromCodeCoverage]
	internal static class EncryptExtension
	{
		public static string Decrypt(this string? secretName)
		{
			var plaintext = string.Empty;
			try
			{
				if (string.IsNullOrEmpty(secretName))
				{
					return string.Empty;
				}

				if (ConfigurationStruct.SiKeyVault ==
					Environment.GetEnvironmentVariable("SiKeyVault"))
				{
					var keyName = Environment.GetEnvironmentVariable("keyName");
					var ClientId = Environment.GetEnvironmentVariable("ClientId");
					var ClientSecret =
						Environment.GetEnvironmentVariable("ClientSecret");
					var vTuAzureTenantID =
						Environment.GetEnvironmentVariable("TuAzureTenantID");

					using (var cancellationTokenSource =
						   new CancellationTokenSource(5000))
					{
						var clientCredential =
							new ClientSecretCredential(vTuAzureTenantID, ClientId,
								ClientSecret);
						var secretClient = new SecretClient(
							new Uri($"https://{keyName}.vault.azure.net/"),
							clientCredential);

						KeyVaultSecret secretNew =
							secretClient.GetSecret(secretName,
								cancellationToken: cancellationTokenSource.Token);
						plaintext = secretNew.Value;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(secretName))
					{
						return string.Empty;
					}

					var Key = Convert.FromBase64String(AesEncryptKey.Key);
					var IV = Convert.FromBase64String(AesEncryptKey.IV);

					var cipherText = Convert.FromBase64String(secretName);

					using (var aesAlg = Aes.Create())
					{
						aesAlg.Mode = CipherMode.CBC;
						aesAlg.KeySize = 256;
						aesAlg.BlockSize = 128;
						aesAlg.Key = Key;
						aesAlg.IV = IV;
						var decryptor =
							aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
						using MemoryStream msDecrypt = new(cipherText);
						using CryptoStream csDecrypt = new(msDecrypt, decryptor,
							CryptoStreamMode.Read);
						using StreamReader srDecrypt = new(csDecrypt);
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
			catch (Exception)
			{
				throw new GeneralException(BaseRepositoryMessages.ERRBSRPSTR02);
			}

			return plaintext;
		}
	}
}
