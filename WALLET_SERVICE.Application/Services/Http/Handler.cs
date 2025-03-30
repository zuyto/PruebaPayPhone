// <copyright file="Handler.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Net;

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Application.Services.Http
{
	/// <summary>
	///     Handler
	/// </summary>
	internal static class Handler
	{
		#region Methods

		public static async Task<T?> Deserialize<T>(HttpResponseMessage response,
			string contentType = "application/json")
		{
			return response.StatusCode
				switch
			{
				HttpStatusCode.NoContent => default,
				HttpStatusCode.UnprocessableEntity or
					HttpStatusCode.NotFound or
					HttpStatusCode.OK => await CreateResponseOk<T>(response,
						contentType),
				_ => throw new HttpRequestException(string.Format(ConfigurationStruct.HttpRequestExceptionMessage, response.StatusCode, response.Headers, await response.Content.ReadAsStringAsync()))
			};
		}

		public static async Task<T?> CreateResponseOk<T>(
			HttpResponseMessage response, string contentType)
		{
			var value = await response.Content.ReadAsStringAsync();

			var respuesta = JsonConvert.DeserializeObject<T>(value,
				new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
				});
			return respuesta;
		}

		public static HttpRequestMessage CreateRequest(HttpMethod method,
			string uriString, string contentTypeSuport = "application/json",
			string? SubscriptionKey = null)
		{
			Uri uri = new(uriString);
			HttpRequestMessage request = new(method, uri);
			request.Headers.Add(ConfigurationStruct.Accept, contentTypeSuport);
			if (!string.IsNullOrEmpty(SubscriptionKey))
			{
				request.Headers.Add(
					ConfigurationStruct.HeadersOcpApimSubscriptionKey,
					SubscriptionKey);
			}

			return request;
		}

		#endregion Methods
	}
}
