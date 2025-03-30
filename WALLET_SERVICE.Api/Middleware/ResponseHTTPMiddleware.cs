// <copyright file="ResponseHTTPMiddleware.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Api.Middleware
{
	/// <summary>
	///     CustomMiddlewareExtensions
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class CustomMiddlewareExtensions
	{
		/// <summary>
		///     UseRequestResponseLogging
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseRequestResponseLogging(
			this IApplicationBuilder app)
		{
			return app.UseMiddleware<RequestResponseLoggerMiddleware>();
		}
	}

	/// <summary>
	///     RequestResponseLoggerMiddleware
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class RequestResponseLoggerMiddleware
	{
		private readonly bool _isRequestResponseLoggingEnabled;
		private readonly RequestDelegate _next;

		/// <summary>
		///     RequestResponseLoggerMiddleware
		/// </summary>
		/// <param name="next"></param>
		/// <param name="config"></param>
		public RequestResponseLoggerMiddleware(RequestDelegate next,
			IConfiguration config)
		{
			_next = next;
			_isRequestResponseLoggingEnabled =
				config.GetValue("EnableRequestResponseLogging", false);
		}

		/// <summary>
		///     InvokeAsync
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		public async Task InvokeAsync(HttpContext httpContext)
		{
			var avoid = false;
			switch (((DefaultHttpContext)httpContext).Request.Path.Value)
			{
				case "/index.html":
				case "/favicon.ico":
				case "/swagger/v1/swagger.json":
					avoid = true;
					break;
			}


			// El middleware está habilitado solo cuando se establece el valor de configuración de EnableRequestResponseLogging y evitar request del swagger.
			if (_isRequestResponseLoggingEnabled && !avoid)
			{
				Console.WriteLine($"HTTP request information:\n" +
								  $"\tMethod: {httpContext.Request.Method}\n" +
								  $"\tPath: {httpContext.Request.Path}\n" +
								  $"\tQueryString: {httpContext.Request.QueryString}\n" +
								  $"\tHeaders: {FormatHeaders(httpContext.Request.Headers)}\n" +
								  $"\tSchema: {httpContext.Request.Scheme}\n" +
								  $"\tHost: {httpContext.Request.Host}\n" +
								  $"\tBody: {await ReadBodyFromRequest(httpContext.Request)}");

				// Reemplace temporalmente el HttpResponseStream, que es un flujo de solo escritura, con un MemoryStream para capturar su valor en curso.
				var originalResponseBody = httpContext.Response.Body;
				using var newResponseBody = new MemoryStream();
				httpContext.Response.Body = newResponseBody;

				// Llamada next middleware en el pipeline
				await _next(httpContext);

				newResponseBody.Seek(0, SeekOrigin.Begin);
				var responseBodyText =
					await new StreamReader(httpContext.Response.Body)
						.ReadToEndAsync();

				Console.WriteLine($"HTTP request information:\n" +
								  $"\tStatusCode: {httpContext.Response.StatusCode}\n" +
								  $"\tContentType: {httpContext.Response.ContentType}\n" +
								  $"\tHeaders: {FormatHeaders(httpContext.Response.Headers)}\n" +
								  $"\tBody: {responseBodyText}");

				newResponseBody.Seek(0, SeekOrigin.Begin);
				await newResponseBody.CopyToAsync(originalResponseBody);
			}
			else
			{
				await _next(httpContext);
			}
		}

		/// <summary>
		///     FormatHeaders
		/// </summary>
		/// <param name="headers"></param>
		/// <returns></returns>
		private static string FormatHeaders(IHeaderDictionary headers)
		{
			return string.Join(", ", headers.Select(kvp =>
			{
				var values = kvp.Value as IEnumerable<string>;
				return $"{{{kvp.Key}: {string.Join(", ", values)}}}";
			}));
		}

		/// <summary>
		///     ReadBodyFromRequest
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private static async Task<string> ReadBodyFromRequest(HttpRequest request)
		{
			//Asegúrese de que el cuerpo de la solicitud se pueda leer varias veces(para el próximo middleware en la canalización).
			request.EnableBuffering();

			using var streamReader =
				new StreamReader(request.Body, leaveOpen: true);
			var requestBody = await streamReader.ReadToEndAsync();

			//Restablezca la posición del flujo del cuerpo de la solicitud para el siguiente middleware en la canalización.
			request.Body.Position = 0;
			return requestBody;
		}
	}
}
