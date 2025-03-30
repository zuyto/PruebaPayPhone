// <copyright file="HandlerTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Net;

using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Application.Services.Http;

namespace WALLET_SERVICE.UnitTests.Application.Services.Http
{
	public class HandlerTests
	{
		[Fact]
		public void CreateRequest_Should_Return_HttpRequestMessage_With_Correct_Headers()
		{
			// Arrange
			var method = HttpMethod.Get;
			var uriString = "https://example.com";
			var contentTypeSupport = "application/json";
			var subscriptionKey = "1234567890";

			var expectedUri = new Uri(uriString);
			var expectedRequest = new HttpRequestMessage(method, expectedUri);
			expectedRequest.Headers.Add(ConfigurationStruct.Accept, contentTypeSupport);
			expectedRequest.Headers.Add(ConfigurationStruct.HeadersOcpApimSubscriptionKey, subscriptionKey);

			// Act
			var actualRequest = Handler.CreateRequest(method, uriString, contentTypeSupport, subscriptionKey);

			// Assert
			Assert.Equal(expectedRequest.Method, actualRequest.Method);
			Assert.Equal(expectedRequest.RequestUri, actualRequest.RequestUri);
			Assert.Equal(expectedRequest.Headers.Accept.ToString(), actualRequest.Headers.Accept.ToString());
			Assert.Equal(expectedRequest.Headers.GetValues(ConfigurationStruct.HeadersOcpApimSubscriptionKey).ToString(), actualRequest.Headers.GetValues(ConfigurationStruct.HeadersOcpApimSubscriptionKey).ToString());
		}
		[Fact]
		public async Task CreateResponseOk_WithJsonContentType_ReturnsDeserializedObject()
		{
			// Arrange
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent("{\"name\":\"John\",\"age\":30}");
			var contentType = "application/json";

			// Act
			var result = await Handler.CreateResponseOk<MyModel>(response, contentType);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.Name);
			Assert.Equal(30, result.Age);
		}
		[Fact]
		public async Task Deserialize_WithNoContentStatusCode_ReturnsDefault()
		{
			// Arrange
			var response = new HttpResponseMessage(HttpStatusCode.NoContent);

			// Act
			var result = await Handler.Deserialize<object>(response);

			// Assert
			Assert.Null(result);
		}

		[Theory]
		[InlineData(HttpStatusCode.UnprocessableEntity)]
		[InlineData(HttpStatusCode.NotFound)]
		[InlineData(HttpStatusCode.OK)]
		public async Task Deserialize_WithValidStatusCodes_ReturnsDeserializedObject(HttpStatusCode statusCode)
		{
			// Arrange
			var response = new HttpResponseMessage(statusCode);
			var content = "{'name': 'John', 'age': 30}";
			response.Content = new StringContent(content);

			// Act
			var result = await Handler.Deserialize<MyModel>(response);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.Name);
			Assert.Equal(30, result.Age);
		}

		[Fact]
		public async Task Deserialize_WithInvalidStatusCode_ThrowsHttpRequestException()
		{
			// Arrange
			var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
			var content = "Bad request";
			response.Content = new StringContent(content);

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(async () =>
			{
				await Handler.Deserialize<object>(response);
			});
		}

		public class MyModel
		{
			public string? Name { get; set; }
			public int Age { get; set; }
		}
	}
}
