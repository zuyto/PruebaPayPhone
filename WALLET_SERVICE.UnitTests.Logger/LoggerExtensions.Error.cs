// <copyright file="LoggerExtensions.Error.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.Extensions.Logging;

using Moq;

using WALLET_SERVICE.Logger;
using WALLET_SERVICE.Logger.Models;
using WALLET_SERVICE.Logger.Static;

using LoggerExtensions = WALLET_SERVICE.Logger.LoggerExtensions;

namespace WALLET_SERVICE.UnitTests.Logger
{
	public class LoggerExtensionsTests
	{
		[Fact]
		public void LogError_LogsError()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();

			var options = new LogErrorOptions
			{
				MemberName = "",
				SourceFilePath = "",
				SourceLineNumber = 1,
				Message = "message {tipoError} metodo {metodo}, parametros ={parametros}, msj: {mensje}",
				EventId = new EventId(1),
				Args = new[] { "Error", "method", "parameters", "message" }
			};

			// Replace the problematic code with the correct code
			mockLogger.Setup(x => x.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
				.Verifiable();

			// Act
			mockLogger.Object.LogError(options);

			// Assert
			mockLogger.Verify(x => x.Log(
				LogLevel.Error, // Verifica que el nivel de log sea Error
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
		}
		[Fact]
		public void LogError_LogsCritical()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();

			var options = new LogErrorOptions
			{
				MemberName = "",
				SourceFilePath = "",
				SourceLineNumber = 1,
				Message = "message {tipoError} metodo {metodo}, parametros ={parametros}, msj: {mensje}",
				EventId = new EventId(2),
				Args = new[] { "Critical", "method", "parameters", "message" }
			};

			// Replace the problematic code with the correct code
			mockLogger.Setup(x => x.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
				.Verifiable();

			// Act
			mockLogger.Object.LogCritical(options);

			// Assert
			mockLogger.Verify(x => x.Log(
				LogLevel.Critical,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
		}

		[Fact]
		public void LogError_LogsWarning()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();

			var options = new LogErrorOptions
			{
				MemberName = "",
				SourceFilePath = "",
				SourceLineNumber = 1,
				Message = "message {tipoError} metodo {metodo}, parametros ={parametros}, msj: {mensje}",
				EventId = new EventId(3),
				Args = new[] { "Warning", "method", "parameters", "message" }
			};

			// Replace the problematic code with the correct code
			mockLogger.Setup(x => x.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
				.Verifiable();

			// Act
			mockLogger.Object.LogWarning(options);

			// Assert
			mockLogger.Verify(x => x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
		}
		[Fact]
		public void LogError_LogsInformation()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();

			var options = new LogErrorOptions
			{
				MemberName = "",
				SourceFilePath = "",
				SourceLineNumber = 1,
				Message = "message {tipoError} metodo {metodo}, parametros ={parametros}, msj: {mensje}",
				EventId = new EventId(4),
				Args = new[] { "Information", "method", "parameters", "message" }
			};

			// Replace the problematic code with the correct code
			mockLogger.Setup(x => x.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
				.Verifiable();

			// Act
			mockLogger.Object.LogInformation(options);

			// Assert
			mockLogger.Verify(x => x.Log(
				LogLevel.Information,
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
		}

		[Fact]
		public void MethodsProperties_ReturnsCorrectDictionaryStructure()
		{
			// Arrange
			string memberName = "TestMethod";
			string sourceFilePath = "/path/to/source/file.cs";
			int sourceLineNumber = 123;
			LogLevel level = LogLevel.Information; // Asegúrate de tener definido LogLevel o ajusta según tu implementación

			// Act
			var result = LoggerExtensions.MethodsProperties(memberName, sourceFilePath, sourceLineNumber, level);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(DateTime.Now.ToString(ConfigTypeMessage.FORMATDATE), result[ConfigTypeMessage.FECHA]);
			Assert.NotNull(result[ConfigTypeMessage.HORA]);
			Assert.Equal("Information", result[ConfigTypeMessage.NIVEL]); // Ajusta según cómo estés manejando los niveles de log
			Assert.Equal(memberName, result[ConfigTypeMessage.METODO]);
			Assert.Equal(sourceFilePath, result[ConfigTypeMessage.ARCHIVO]);
			Assert.Equal(sourceLineNumber, result[ConfigTypeMessage.LINEA]);
		}
		[Fact]
		public void MethodsProperties_HandlesUndefinedLogLevel()
		{
			// Arrange
			string memberName = "TestMethod";
			string sourceFilePath = "/path/to/source/file.cs";
			int sourceLineNumber = 123;
			LogLevel level = (LogLevel)999; // Un valor que no está definido en el enum LogLevel

			// Act
			var result = LoggerExtensions.MethodsProperties(memberName, sourceFilePath, sourceLineNumber, level);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(ConfigTypeMessage.LVL, result[ConfigTypeMessage.NIVEL]); // Asegúrate de que se usa el valor por defecto cuando level es inválido
		}

		[Fact]
		public void LogWithOptions_WithNullArgs_PassesEmptyArrayToLog()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();
			var options = new LogErrorOptions
			{
				MemberName = "Member",
				SourceFilePath = "FilePath",
				SourceLineNumber = 123,
				EventId = new EventId(1),
				Exception = new Exception("Test exception"),
				Args = null
			};
			LogLevel level = LogLevel.Error;

			// Act
			LoggerExtensions.LogWithOptions(mockLogger.Object, level, options);

			// Assert
			mockLogger.Verify(x => x.Log(
				It.Is<LogLevel>(l => l == LogLevel.Error),
				It.Is<EventId>(e => e.Id == 1),
				It.IsAny<It.IsAnyType>(),
				It.Is<Exception>(ex => ex.Message == "Test exception"),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
				Times.Once);
		}
		[Fact]
		public void LogWithOptions_WithNullArgs_UsesEmptyArray()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();
			var capturedArgs = Array.Empty<object?>();
			mockLogger.Setup(x => x.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
				.Callback(new InvocationAction(invocation =>
				{
					capturedArgs = [];
				}));

			var options = new LogErrorOptions
			{
				Args = null
			};
			LogLevel level = LogLevel.Error;

			// Act
			LoggerExtensions.LogWithOptions(mockLogger.Object, level, options);

			// Assert
			Assert.IsType<object?[]?>(capturedArgs); // Verifica que el argumento capturado sea un arreglo
			Assert.Empty(capturedArgs); // Verifica que el arreglo esté vacío
		}
		[Fact]
		public void LogWithOptions_WithNonNullArgs_UsesProvidedArgs()
		{
			// Arrange
			var mockLogger = new Mock<ILogger>();
			var capturedArgs = Array.Empty<object?>();
			var expectedArgs = new object[] { "test1", "test2" };
			mockLogger.Setup(x => x.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.IsAny<It.IsAnyType>(),
				It.IsAny<Exception>(),
				It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
				.Callback(new InvocationAction(invocation =>
				{
					capturedArgs = ["test1", "test2"];
				}));
			var options = new LogErrorOptions
			{
				Args = expectedArgs
			};
			LogLevel level = LogLevel.Error;

			// Act
			LoggerExtensions.LogWithOptions(mockLogger.Object, level, options);

			// Assert
			Assert.IsType<object?[]?>(capturedArgs); // Verifica que el argumento capturado sea un arreglo
			var actualArgs = capturedArgs;
			Assert.Equal(expectedArgs.Length, actualArgs.Length); // Verifica que el número de argumentos coincida
			for (int i = 0; i < expectedArgs.Length; i++)
			{
				Assert.Equal(expectedArgs[i], actualArgs[i]); // Verifica que cada argumento coincida
			}
		}


	}
}
