// <copyright file="PingServicesTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Reflection;

using Microsoft.Extensions.Options;

using Moq;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Application.Services.Ping;
using WALLET_SERVICE.Domain;

namespace WALLET_SERVICE.UnitTests.Application.Services.ping
{
	public class PingServicesTest
	{
		private readonly Mock<IOptions<AppSettings>> mockappsettings;
		private readonly Mock<IDynamicRepository> mockDinamicRepository;
		private readonly Mock<ISerilogImplements> mockSerilogImplements;

		private readonly Mock<IUnitOfWorkDynamic> mockIUnitOfWorkDynamic;

		private readonly MockRepository mockRepository;

		public PingServicesTest()
		{
			mockRepository = new MockRepository(MockBehavior.Strict);
			mockIUnitOfWorkDynamic = mockRepository.Create<IUnitOfWorkDynamic>();
			mockDinamicRepository = new Mock<IDynamicRepository>();
			mockappsettings = new Mock<IOptions<AppSettings>>();
			mockSerilogImplements = new Mock<ISerilogImplements>();
		}

		private PingServices CreatePingServices()
		{
			mockappsettings.Setup(x => x.Value).Returns(new AppSettings
			{
				ConnectionStrings = new ConnectionStrings { SecretDB = "SGLPROD" }
			});
			mockIUnitOfWorkDynamic.Setup(x => x.DinamicRepository)
				.Returns(mockDinamicRepository.Object);

			return new PingServices(mockIUnitOfWorkDynamic.Object,
				mockSerilogImplements.Object, mockappsettings.Object);
		}

		[Fact]
		public async Task PruebaConexionSGLPROD_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			string? capturedType = null;
			string? capturedMethod = null;
			string? capturedParameters = null;
			string? capturedMessage = null;
			string? CapturememberName = null;
			string? CaptureFile = null;
			int CaptureLine = 0;

			mockSerilogImplements
				.Setup(x => x.ObtainMessageDefault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Callback<string, string, string?, string?, string, string, int>((type, method, parameters, message, memberName, file, line) =>
				{
					capturedType = type;
					capturedMethod = method;
					capturedParameters = parameters;
					capturedMessage = message;
					CapturememberName = memberName;
					CaptureFile = file;
					CaptureLine = line;
				});

			var pingServices = CreatePingServices();

			// Act
			var result = await pingServices.SecretConexion("SglProd");

			// Assert

			Assert.Equal(ConfigurationMessageType.Information, capturedType);
			Assert.Equal(MetodosMessage.pingSecretConexion, capturedMethod);
			Assert.NotNull(capturedParameters);
			Assert.Null(capturedMessage);
			Assert.Equal("SecretConexion", CapturememberName);
			Assert.Contains("PingServices.cs", CaptureFile);
			Assert.True(CaptureLine > 0);

			Assert.NotNull(result);
			Assert.IsAssignableFrom<string?>(result);
			mockRepository.VerifyAll();
		}

		[Fact]
		public async Task PruebaConexionSGLPROD_StateUnderTest_Null()
		{
			// Arrange
			var pingServices = CreatePingServices();

			// Act
			var result = await pingServices.SecretConexion("SglProd");
			// Assert
			Assert.True(string.IsNullOrEmpty(result));
			Assert.IsAssignableFrom<string?>(result);
			mockRepository.VerifyAll();
		}

		[Fact]
		public async Task PruebaConexionOther_StateUnderTest_Null()
		{
			// Arrange
			var pingServices = CreatePingServices();

			// Act
			var result = await pingServices.SecretConexion("UNIPROD");
			// Assert
			Assert.True(string.IsNullOrEmpty(result));
			Assert.IsAssignableFrom<string?>(result);
			mockRepository.VerifyAll();
		}

		[Fact]
		public void PruebaGetAppsettings_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var pingServices = CreatePingServices();
			// Act
			var result = pingServices.GetAppsettings();
			// Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<AppSettings?>(result);
			mockRepository.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task Version_CallsObtainMessageDefaultWithExpectedParameters()
		{
			// Variables para capturar los argumentos pasados al método ObtainMessageDefault
			string? capturedType = null;
			string? capturedMethod = null;
			string? capturedParameters = null;
			string? capturedMessage = null;
			string? CapturememberName = null;
			string? CaptureFile = null;
			int CaptureLine = 0;

			// Configura el mock con un Callback
			mockSerilogImplements
				.Setup(x => x.ObtainMessageDefault(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Callback<string, string, string?, string?, string, string, int>((type, method, parameters, message, memberName, file, line) =>
				{
					capturedType = type;
					capturedMethod = method;
					capturedParameters = parameters;
					capturedMessage = message;
					CapturememberName = memberName;
					CaptureFile = file;
					CaptureLine = line;
				});

			// Otras configuraciones de mock y creación de la instancia de PingServices
			var pingServices = CreatePingServices();

			// Act
			var assembly = Assembly.GetExecutingAssembly();
			var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			var expectedVersion = fvi.FileVersion;

			var version = await pingServices.Version();

			// Assert
			// Ahora puedes hacer aserciones sobre las variables capturadas para verificar si los argumentos son los esperados
			Assert.Equal(ConfigurationMessageType.Information, capturedType);
			Assert.Equal(MetodosMessage.pingVersion, capturedMethod);
			Assert.Null(capturedParameters);
			Assert.Null(capturedMessage);
			Assert.Equal("Version", CapturememberName);
			Assert.Contains("PingServices.cs", CaptureFile);
			Assert.True(CaptureLine > 0);
			Assert.Equal(expectedVersion, version);
		}
		[Fact]
		public async Task SecretConexion_DbSecretDB_ReturnsExpectedResult()
		{
			// Arrange
			var expectedDatabaseName = "ExpectedDatabaseName";
			var mockResult = Task.FromResult(new List<object>
					{
						new Dictionary<string, object> { { ConfigurationStruct.OraDatabaseName, expectedDatabaseName } }
					});

			mockDinamicRepository
				.Setup(x => x.ExecuteSentenciaOnDatabase(ConfigurationStruct.sentenseDbOracle, It.IsAny<string>()))
				.Returns(mockResult);

			var pingServices = CreatePingServices();

			// Act
			var result = await pingServices.SecretConexion(ConfigurationStruct.DbSecretDB);

			// Assert
			Assert.Equal(expectedDatabaseName, result);
			mockRepository.VerifyAll();
		}

	}
}
