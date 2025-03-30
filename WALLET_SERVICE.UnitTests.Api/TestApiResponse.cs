// <copyright file="TestApiResponse.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Api.Response;
using WALLET_SERVICE.Application.Common.CustomEntities;
using WALLET_SERVICE.Application.Common.Helpers;
using WALLET_SERVICE.Application.Common.Models.DTOs;

namespace WALLET_SERVICE.UnitTests.Api
{
	public class TestApiResponse
	{
		[Fact]
		public void ApiResponse_Constructor()
		{
			var obj = new ApiResponse<dynamic>();
			Assert.NotNull(obj);
		}

		[Fact]
		public void TestApiResponseSuccesfull()
		{
			var obj = ApiResponse<object>.CreateSuccessful(new object());
			Assert.NotNull(obj);
		}

		[Fact]
		public void TestApiResponseUnsuccesfull()
		{
			var obj = ApiResponse<string>.CreateUnsuccessful(
				new List<string> { "Message1", "Message2", "Message3" });
			Assert.NotNull(obj);
		}

		[Fact]
		public void TestApiResponseUnsuccesfullTwo()
		{
			var result = GenericHelpers.BuildResponse<DtoTransferJsonResponse>(false, null, "Mensaje error");

			var obj = ApiResponse<dynamic>.CreateUnsuccessful(result, "Mensaje error");
			Assert.NotNull(obj);
		}

		[Fact]
		public void TestApiResponseError()
		{
			var obj = ApiResponse<object>.CreateError("Error en la aplicación");
			Assert.NotNull(obj);
		}

		[Fact]
		public void TestApiResponseErrorTwo()
		{
			var result = GenericHelpers.BuildResponse<DtoTransferJsonResponse>(false, null, "Mensaje error");
			var obj = ApiResponse<object>.CreateError("Error en la aplicación", result);
			Assert.NotNull(obj);
		}


		[Fact]
		public void ApiResponseBase_Constructor_SetsPropertiesCorrectly()
		{
			// Arrange
			var errorMessage = "Error message";
			var isError = true;
			var isSuccessful = false;
			IEnumerable<string> messages =
				new List<string> { "Message 1", "Message 2" };
			var meta = new Metadata();
			var result = 42;

			// Act
			var response = new ApiResponseBase<int>
			{
				ErrorMessage = errorMessage,
				IsError = isError,
				IsSuccessful = isSuccessful,
				Messages = messages,
				Meta = meta,
				Result = result
			};

			// Assert
			Assert.Same(errorMessage, response.ErrorMessage);
			Assert.True(isError);
			Assert.True(response.IsError);
			Assert.False(isSuccessful);
			Assert.False(response.IsSuccessful);
			Assert.Same(messages, response.Messages);
			Assert.Same(meta, response.Meta);
			Assert.Equal(result, response.Result);
		}

		[Fact]
		public void DefaultConstructor_Initializes_WithDefaultValues()
		{
			// Act
			var response = new ApiResponse<object>();

			// Assert
			Assert.False(response.IsSuccessful);
			Assert.False(response.IsError);
			Assert.Null(response.ErrorMessage);
			Assert.Null(response.Messages);
			Assert.Null(response.Result);
		}

		[Fact]
		public void ParameterizedConstructor_Initializes_PropertiesCorrectly()
		{
			// Arrange
			var isSuccessful = true;
			var isError = false;
			var errorMessage = "Error Message";
			var messages = new List<string> { "Message1", "Message2" };
			var result = "Test Result";

			// Act
			var response = new ApiResponse<string>(isSuccessful, isError,
				errorMessage, messages, result);

			// Assert
			Assert.Equal(isSuccessful, response.IsSuccessful);
			Assert.Equal(isError, response.IsError);
			Assert.Equal(errorMessage, response.ErrorMessage);
			Assert.Equal(messages, response.Messages);
			Assert.Equal(result, response.Result);
		}
	}
}
