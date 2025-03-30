// <copyright file="SerilogImplementsTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.Extensions.Logging;

using Moq;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Application.Services.Serilog;
using WALLET_SERVICE.Logger.Models;

namespace WALLET_SERVICE.UnitTests.Application.Services.Serilog
{
	public class SerilogImplementsTests
	{
		private readonly MockRepository mockRepository;
		private readonly Mock<ISerilogImplements> mockSerilogServices;
		private readonly Mock<ILogger<SerilogImplements>> mockLogger;
		public SerilogImplementsTests()
		{
			mockRepository = new MockRepository(MockBehavior.Strict);
			mockSerilogServices = new Mock<ISerilogImplements>();
			mockLogger = new Mock<ILogger<SerilogImplements>>();
		}

		[Fact]
		public void Constructor_ShouldNotBeNull()
		{
			// Arrange
			var loggerMock = new Mock<ILogger<SerilogImplements>>();

			// Act
			var serilogImplements = new SerilogImplements(loggerMock.Object);

			// Assert
			Assert.NotNull(serilogImplements);
		}
		[Fact]
		public void Test_ObtainMessageDefault_WithMessageTypeError()
		{
			// Arrange
			var loggerMock = new Mock<ILogger<SerilogImplements>>();
			var serilogImplements = new SerilogImplements(loggerMock.Object); // Puedes pasar null como argumento para ILogger<SerilogImplements> ya que no se utilizará en esta prueba
			var messageType = ConfigurationMessageType.Error;
			var method = "TestMethod";
			var parameters = "TestParameters";
			var message = "TestMessage";

			// Act
			var result = serilogImplements.ObtainMessageDefault(messageType, method, parameters, message);

			// Assert
			Assert.Equal(message, result);
		}

		[Fact]
		public void TestObtainMessageDefaultFunction_Error()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<SerilogImplements>>();
			var serilogImplements = new SerilogImplements(mockLogger.Object);
			string messageType = ConfigurationMessageType.Error;
			string method = "TestMethod";
			string parameters = "TestParameters";
			string message = "TestMessage";

			var options = new LogErrorOptions
			{
				MemberName = "",
				SourceFilePath = "",
				SourceLineNumber = 1,
				Message = "message {tipoError} metodo {metodo}, parametros ={parametros}, msj: {mensje}",
				EventId = new EventId(1),
				Args = new[] { "Error", "method", "parameters", "message" }
			};

			// Act
			var result = serilogImplements.ObtainMessageDefault(messageType, method, parameters, message);

			// Assert
			Assert.Equal(message, result);
		}

	}
}
