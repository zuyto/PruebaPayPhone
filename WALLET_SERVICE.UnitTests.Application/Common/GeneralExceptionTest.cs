// <copyright file="GeneralExceptionTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Exceptions;

namespace WALLET_SERVICE.UnitTests.Application.Common
{
	public class GeneralExceptionTest
	{
		[Fact]
		public void
			GeneralException_DefaultConstructor_SetsMessageToDefaultMessage()
		{
			// Arrange
			var exception = new GeneralException();

			// Act

			// Assert
			Assert.Equal("La entidad no existe.", exception.Message);
		}

		[Fact]
		public void GeneralException_ConstructorWithMessage_SetsMessageCorrectly()
		{
			// Arrange
			var customMessage = "Custom exception message";
			var exception = new GeneralException(customMessage);

			// Act

			// Assert
			Assert.Equal(customMessage, exception.Message);
		}

		[Fact]
		public void
			GeneralException_ConstructorWithMessageAndInnerException_SetsMessageAndInnerExceptionCorrectly()
		{
			// Arrange
			var customMessage = "Custom exception message";
			var innerException = new Exception("Inner exception message");
			var exception = new GeneralException(customMessage, innerException);

			// Act

			// Assert
			Assert.Equal(customMessage, exception.Message);
			Assert.Same(innerException, exception.InnerException);
		}

		[Fact]
		public void ToJson_ReturnsValidJson()
		{
			// Arrange
			var exception = new GeneralException("Custom message");
			var expectedJson = JsonConvert.SerializeObject(exception, Formatting.Indented);

			// Act
			var resultJson = exception.ToJson();

			// Assert
			Assert.Equal(expectedJson, resultJson);
		}

		[Fact]
		public void FromJson_ReturnsOriginalInstance()
		{
			var customMessage = "Custom message";
			var exception = new GeneralException(customMessage);
			var json = JsonConvert.SerializeObject(exception);

			// Act
			var deserializedException = GeneralException.FromJson(json);

			// Assert
			Assert.IsType<GeneralException>(deserializedException);
			Assert.Equal(customMessage, deserializedException.Message);
		}
	}
}
