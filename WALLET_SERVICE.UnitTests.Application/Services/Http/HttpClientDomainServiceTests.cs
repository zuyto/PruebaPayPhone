// <copyright file="HttpClientDomainServiceTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Net;
using System.Reflection;

using Moq;
using Moq.Protected;

using WALLET_SERVICE.Application.Services.Http;

namespace WALLET_SERVICE.UnitTests.Application.Services.Http
{
	public class HttpClientDomainServiceTests
	{
		[Fact]
		public void Constructor_SetsHttpClientTimeoutCorrectly()
		{
			// Crear instancia de HttpClientDomainService
			var service = new HttpClientDomainService();

			// Usar reflexión para obtener el campo _client
			var client = service.GetType().GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(service) as HttpClient;

			// Verificar que el timeout se haya configurado correctamente
			Assert.Equal(TimeSpan.FromMinutes(5), client?.Timeout);
		}
		[Fact]
		public async Task SendAsync_ReturnsExpectedResponse()
		{
			// Mock del HttpMessageHandler
			var mockHandler = new Mock<HttpMessageHandler>();

			mockHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent("Test Response")
				});

			// Instancia de HttpClient con el handler mockeado
			var client = new HttpClient(mockHandler.Object)
			{
				Timeout = TimeSpan.FromMinutes(5)
			};

			// Crear instancia de HttpClientDomainService y usar reflexión para establecer el cliente mockeado
			var service = new HttpClientDomainService();
			var clientField = typeof(HttpClientDomainService).GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance);
			clientField?.SetValue(service, client);

			// Preparar la llamada al método SendAsync
			var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");
			var cancellationToken = new CancellationToken();

			// Actuar: Llamar al método SendAsync
			var result = await service.SendAsync<object>(request, cancellationToken);

			// Afirmar que la respuesta es la esperada
			Assert.Equal(HttpStatusCode.OK, result.StatusCode);

			// Verificar que se llamó a SendAsync del HttpMessageHandler mockeado
			mockHandler.Protected().Verify(
				"SendAsync",
				Times.Once(),
				ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
				ItExpr.IsAny<CancellationToken>()
			);
		}
	}
}
