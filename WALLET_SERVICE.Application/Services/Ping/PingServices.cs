// <copyright file="PingServices.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Reflection;

using Microsoft.Extensions.Options;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Ping;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Domain;

namespace WALLET_SERVICE.Application.Services.Ping
{
	internal class PingServices : IPingServices
	{
		private readonly AppSettings _appsettings;
		private readonly ISerilogImplements _SerilogImplements;
		private readonly IUnitOfWorkDynamic unitOfWorkDynamic;

		public PingServices(IUnitOfWorkDynamic unitOfWorkDynamic,
			ISerilogImplements serilogImplements, IOptions<AppSettings> appsettings)
		{
			this.unitOfWorkDynamic = unitOfWorkDynamic;
			_appsettings = appsettings.Value;
			_SerilogImplements = serilogImplements;
		}

		/// <summary>
		///     Version de Api
		/// </summary>
		/// <returns></returns>
		public async Task<string?> Version()
		{
			_SerilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, MetodosMessage.pingVersion, null, null);
			return await Task.Run(() =>
			{
				var assembly = Assembly.GetExecutingAssembly();
				var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
				var version = fvi.FileVersion;
				return version;
			});
		}

		/// <summary>
		/// </summary>
		/// <param name="database"></param>
		/// <returns></returns>
		public async Task<string?> SecretConexion(string secreto)
		{
			_SerilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, MetodosMessage.pingSecretConexion, secreto, null);
			var SecretDB = Environment.GetEnvironmentVariable(ConfigurationStruct.DbSecretDB);
			SecretDB = SecretDB == null ? ConfigurationStruct.SecretDb : SecretDB;
			switch (secreto)
			{
				case ConfigurationStruct.DbSecretDB:
					var resultSGLPROD =
						await unitOfWorkDynamic.DinamicRepository
							.ExecuteSentenciaOnDatabase(
								ConfigurationStruct.sentenseDbOracle,
								SecretDB);
					var singleSGLPROD =
						(IDictionary<string, object>?)
						resultSGLPROD?.FirstOrDefault();
					return singleSGLPROD == null
						? string.Empty
						: singleSGLPROD[ConfigurationStruct.OraDatabaseName]
							.ToString();

				default:
					var result =
						await unitOfWorkDynamic.DinamicRepository
							.ExecuteSentenciaOnDatabase(
								ConfigurationStruct.sentenseDbOracle, secreto);
					var single =
						(IDictionary<string, object>?)result?.FirstOrDefault();
					return single == null
						? ""
						: single[ConfigurationStruct.OraDatabaseName]
							.ToString();
			}
		}

		/// <summary>
		///     Configuración de Appsettings
		/// </summary>
		/// <returns></returns>
		public AppSettings GetAppsettings()
		{
			return _appsettings;
		}
	}

}
