// <copyright file="HttpServiceManager.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Application.Common.Helpers
{
	[ExcludeFromCodeCoverage]

	public class HttpServiceManager
	{

		private readonly IHttpClientFactory _httpClientFactory;

		/// <summary>
		/// Instancia de Logger
		/// </summary>
		private readonly ISerilogImplements _logger;

		public HttpServiceManager(IHttpClientFactory httpClientFactory, ISerilogImplements logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		/// <summary>
		/// Realiza una peticion Post Http asincrona para las trasportadoras
		///
		/// <code>
		/// Ejemplo para consumir el metodo:
		/// 
		/// 
		/// var respuesta = await _httpEndpoint.PostHttpAsync("nombreEndpoint", "Request", "tokenDeAutenticacion");
		/// </code>
		/// </summary>
		/// <param name="nombreEndpoint">Nombre del endpoint a consumir</param>
		/// <param name="request">Parametros u objecto a enviar al servicio</param>
		/// <param name="auth">KEY Token Autorización (Basic ó Bearer) </param>
		/// <returns>una respuesta generica <see cref="DtoGenericResponse"/></returns>
		public virtual async Task<DtoGenericResponseHttp> PostHttpAsync(string nombreEndpoint, string request, string? auth = null)
		{
			DtoGenericResponseHttp res;

			try
			{
				_logger.ObtainMessageDefault(ConfigurationMessageType.Information, request, null, $"\n\n ***** REQUEST QUE RECIBE EL API EXTERNA {nombreEndpoint}******\n\n");


				var clientSgl = _httpClientFactory.CreateClient(nombreEndpoint);
				if (auth != null)
				{
					clientSgl.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
				}

				////clientSgl.Timeout = TimeSpan.FromSeconds(_appSettings.TiempoEsperaApiExterna);
				clientSgl.DefaultRequestHeaders.Accept.Clear();
				clientSgl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ConfigurationStruct._contentTypeSuport));
				StringContent stringContent = new StringContent(request, Encoding.UTF8, ConfigurationStruct._contentTypeSuport);
				HttpResponseMessage respuestaServicio = await clientSgl.PostAsync("", stringContent);
				_logger.ObtainMessageDefault(ConfigurationMessageType.Information, respuestaServicio.Content.ReadAsStringAsync().Result, null, $"\n\n ***** RESPUESTA HTTP {nombreEndpoint}******/\n | RESULT: {respuestaServicio} - CODE: {respuestaServicio.StatusCode}\n\n");

				res = await ValidateResponseHttpAsync(respuestaServicio);
			}
			catch (HttpRequestException ex) when (ex.InnerException is TimeoutException)
			{
				// Captura la excepción de timeout y registra un mensaje en el log
				_logger.ObtainMessageDefault(ConfigurationMessageType.Critical, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, $"\n\n {UserTypeMessages.ERRGEN08} a {nombreEndpoint}. Detalles: {ex.Message}\n\n");
				res = GenericHelpers.BuildResponseHttp(false, UserTypeMessages.ERRGEN08, $"ERROR POST: {ex.Message}", JsonConvert.SerializeObject(ex.HandleExceptionMessage(true)));
			}
			catch (Exception ex)
			{
				_logger.ObtainMessageDefault(ConfigurationMessageType.Critical, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, $"\n\n ***** CATCH HTTP {nombreEndpoint}******/\n | RESULT: {ex.Message}\n\n");
				res = GenericHelpers.BuildResponseHttp(false, GenericHelpers.HandleExceptionMessage(ex, true), $"ERROR POST: {ex.Message}", JsonConvert.SerializeObject(ex.HandleExceptionMessage(true)));
			}
			return res;
		}



		/// <summary>
		/// Realiza una petición GET HTTP asíncrona.
		/// 
		/// <code>
		/// Ejemplo para consumir el metodo:
		/// 
		/// var parametros = new Dictionary&lt;string, string&gt;
		/// {
		///     { "parametro1", "valor1" },
		///     { "parametro2", "valor2" }
		/// };
		/// 
		/// var respuesta = await _httpEndpoint.GetHttpAsync("nombreEndpoint", "tokenDeAutenticacion", parametros);
		/// </code>
		/// 
		/// </summary>
		/// <param name="nombreEndpoint"></param>
		/// <param name="auth">KEY Token  Autorización (Basic ó Bearer)</param>
		/// <returns>una respuesta generica <see cref="DtoGenericResponse"/></returns>
		public virtual async Task<DtoGenericResponseHttp> GetHttpAsync(string nombreEndpoint, string? auth = null, IDictionary<string, string>? parametros = null)
		{
			DtoGenericResponseHttp res;

			try
			{
				var clientFactory = _httpClientFactory.CreateClient(nombreEndpoint);

				// Construir la cadena de consulta si hay parámetros
				string queryString = parametros != null
					? string.Join("&", parametros.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"))
					: "";

				// Agregar la cadena de consulta a la URL inicial
				string url = string.IsNullOrEmpty(queryString) ? "" : $"?{queryString}";

				if (auth != null)
				{
					_logger.ObtainMessageDefault(ConfigurationMessageType.Information, null, null, "\n\n/***** CONSUME ENDPOINT CON AUTENTICACION ******/\n | TIPO: Bearer\n");
					clientFactory.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
				}

				///clientSgl.Timeout = TimeSpan.FromSeconds(_appSettingsDTO.tiempoApiExterna);
				clientFactory.DefaultRequestHeaders.Accept.Clear();
				clientFactory.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ConfigurationStruct._contentTypeSuport));

				HttpResponseMessage respuestaServicio = await clientFactory.GetAsync(url);
				_logger.ObtainMessageDefault(ConfigurationMessageType.Information, respuestaServicio.Content.ReadAsStringAsync().Result, null, $"\n\n ***** RESPUESTA HTTP {nombreEndpoint}******/\n | RESULT: {respuestaServicio} - CODE: {respuestaServicio.StatusCode}\n\n");

				res = await ValidateResponseHttpAsync(respuestaServicio);
			}
			catch (HttpRequestException ex) when (ex.InnerException is TimeoutException)
			{
				// Captura la excepción de timeout y registra un mensaje en el log
				_logger.ObtainMessageDefault(ConfigurationMessageType.Critical, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, $"\n\n {UserTypeMessages.ERRGEN08} a {nombreEndpoint}. Detalles: {ex.Message}\n\n");
				res = GenericHelpers.BuildResponseHttp(false, UserTypeMessages.ERRGEN08, $"ERROR GET: {ex.Message}", JsonConvert.SerializeObject(ex.HandleExceptionMessage(true)));
			}
			catch (Exception ex)
			{
				_logger.ObtainMessageDefault(ConfigurationMessageType.Critical, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, $"\n\n ***** CATCH HTTP {nombreEndpoint}******/\n | RESULT: {ex.Message}\n\n");
				res = GenericHelpers.BuildResponseHttp(false, GenericHelpers.HandleExceptionMessage(ex, true), $"ERROR GET: {ex.Message}", JsonConvert.SerializeObject(ex.HandleExceptionMessage(true)));
			}
			return res;

		}


		/// <summary>
		/// Arma la respuesta del servicio externo para trasportadoras
		/// </summary>
		/// <param name="response">datos de la respuesta del servicio <see cref="HttpResponseMessage"/></param>
		/// <returns>una respuesta generica <see cref="DtoGenericResponseHttp"/></returns>
		private async Task<DtoGenericResponseHttp> ValidateResponseHttpAsync(HttpResponseMessage response)
		{
			string jsonResponse;

			if (response.IsSuccessStatusCode)
			{
				jsonResponse = await GetResponseContent(response);
				_logger.ObtainMessageDefault(ConfigurationMessageType.Warning, jsonResponse, null, $"\n\n/***** RESPUESTA HTTP *****/\n | RESPUESTA OK: {response.StatusCode}\n\n | URL: {response?.RequestMessage?.RequestUri?.AbsoluteUri}\n\n");
				return GenericHelpers.BuildResponseHttp(true, jsonResponse, $"HTTP OK: {response?.StatusCode} | URL: {response?.RequestMessage?.RequestUri?.AbsoluteUri}", jsonResponse);
			}
			else
			{
				jsonResponse = await GetResponseContent(response);
				_logger.ObtainMessageDefault(ConfigurationMessageType.Error, jsonResponse, null, $"\n\n/***** RESPUESTA HTTP *****/\n | RESPUESTA FAIL : {response.StatusCode}\n | URL: {response?.RequestMessage?.RequestUri?.AbsoluteUri}\n\n");
				return GenericHelpers.BuildResponseHttp(false, jsonResponse, $"HTTP FAIL: {response?.StatusCode} | URL: {response?.RequestMessage?.RequestUri?.AbsoluteUri}", jsonResponse);
			}
		}


		/// <summary>
		/// Verifica si la respuesta contiene datos en el Content
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		public static async Task<string> GetResponseContent(HttpResponseMessage response)
		{
			if (response.Content != null && response.Content.Headers.ContentLength > 0)
			{
				return await response.Content.ReadAsStringAsync();
			}
			else
			{
				return JsonConvert.SerializeObject(response);
			}
		}


	}
}
