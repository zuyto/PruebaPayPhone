// <copyright file="HttpClientDomainService.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Interfaces.Services.Http;

namespace WALLET_SERVICE.Application.Services.Http
{
	internal class HttpClientDomainService : IHttpClientDomainService
	{
		#region Variables

		private readonly HttpClient _client;

		#endregion Variables

		#region Constructor

		public HttpClientDomainService()
		{
			_client = new HttpClient();
			_client.Timeout = TimeSpan.FromMinutes(5);
		}

		#endregion Constructor

		#region Methods

		public async Task<HttpResponseMessage> SendAsync<T>(
			HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var response = await _client.SendAsync(request, cancellationToken);

			return response;
		}

		#endregion Methods
	}
}
