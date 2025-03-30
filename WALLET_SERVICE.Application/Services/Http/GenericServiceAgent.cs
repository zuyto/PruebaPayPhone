// <copyright file="GenericServiceAgent.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Text;

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Http;
using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Application.Services.Http
{
	[ExcludeFromCodeCoverage]
	internal class GenericServiceAgent : IGenericServiceAgent
	{
		#region Fields
		private readonly HttpClientDomainService _genericHttpClient = new();

		public async Task<T?> GetAsync<T>(string url,
			string contentType = ConfigurationStruct._contentTypeSuport,
			CancellationToken cancellationToken = default)
		{
			using var request =
				Handler.CreateRequest(HttpMethod.Get, url, contentType);
			var response =
				await _genericHttpClient.SendAsync<T>(request, cancellationToken);

			return await Handler.Deserialize<T>(response, contentType);
		}

		#endregion

		#region Methods

		public async Task<T?> PostAsync<T>(string url, object body,
			string contentType = ConfigurationStruct._contentTypeSuport,
			CancellationToken cancellationToken = default)
		{
			using var request =
				Handler.CreateRequest(HttpMethod.Post, url, contentType);
			if (body != null)
			{
				request.Content = new StringContent(
					JsonConvert.SerializeObject(body), Encoding.UTF8,
					ConfigurationStruct._contentTypeSuport);
			}

			var response =
				await _genericHttpClient.SendAsync<T>(request, cancellationToken);
			return await Handler.Deserialize<T>(response);
		}

		public async Task<T?> PostAsync<T>(string url, object body,
			string? SubscriptionKey = null, string contentType = ConfigurationStruct._contentTypeSuport,
			CancellationToken cancellationToken = default)
		{
			using var request = Handler.CreateRequest(HttpMethod.Post, url,
				contentType, SubscriptionKey);
			if (body != null)
			{
				request.Content = new StringContent(
					JsonConvert.SerializeObject(body), Encoding.UTF8,
					ConfigurationStruct._contentTypeSuport);
			}

			var response =
				await _genericHttpClient.SendAsync<T>(request, cancellationToken);
			return await Handler.Deserialize<T>(response);
		}

		public async Task<T?> PutAsync<T>(string url, object body,
			string contentType = ConfigurationStruct._contentTypeSuport,
			CancellationToken cancellationToken = default)
		{
			using var request =
				Handler.CreateRequest(HttpMethod.Put, url, contentType);
			if (body != null)
			{
				request.Content = new StringContent(
					JsonConvert.SerializeObject(body), Encoding.UTF8, contentType);
			}

			var response =
				await _genericHttpClient.SendAsync<T>(request, cancellationToken);
			return await Handler.Deserialize<T>(response);
		}

		#endregion
	}
}
