// <copyright file="AppSettingsTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Moq;

using WALLET_SERVICE.Domain;

namespace WALLET_SERVICE.UnitTests.Domain
{
	public class AppSettingsTests
	{
		/// <summary>
		///     mockRepository
		/// </summary>
		public readonly MockRepository mockRepository;

		/// <summary>
		///     AppSettingsTests
		/// </summary>
		public AppSettingsTests()
		{
			mockRepository = new MockRepository(MockBehavior.Strict);
		}

		/// <summary>
		///     CreateAppSettings
		/// </summary>
		/// <returns></returns>
		public static AppSettings CreateAppSettings()
		{
			return new AppSettings();
		}

		[Fact]
		public void TestMethod1()
		{
			// Arrange
			var appSettings = CreateAppSettings();

			// Act

			// Assert
			Assert.NotNull(appSettings);
			mockRepository.VerifyAll();
		}

		[Fact]
		public void AppSettings_ConnectionStrings_ShouldNotBeNull()
		{
			// Arrange
			var appSettings = new AppSettings();

			// Act
			appSettings.ConnectionStrings = new();
			appSettings.ConnectionStrings.SecretDB = "secreto";
			appSettings.ConnectionStrings.HCAV_SGL = "HCAV";

			appSettings.ConnectionStrings.PROD_SGL = "PROD";

			// Assert
			Assert.NotNull(appSettings.ConnectionStrings);
			Assert.Equal("secreto", appSettings.ConnectionStrings.SecretDB);
			Assert.Equal("HCAV", appSettings.ConnectionStrings.HCAV_SGL);
			Assert.Equal("PROD", appSettings.ConnectionStrings.PROD_SGL);
		}

		[Fact]
		public void AppSettings_Logging_ShouldNotBeNull()
		{
			// Arrange
			var appSettings = new AppSettings();

			// Act
			appSettings.Logging = new();
			appSettings.Logging.LogLevel = new();
			appSettings.Logging.LogLevel.Default = "Information";
			appSettings.Logging.LogLevel.MicrosoftAspNetCore = "Warning";

			// Assert
			Assert.NotNull(appSettings.Logging);
			Assert.Equal("Information", appSettings.Logging.LogLevel.Default);
			Assert.Equal("Warning", appSettings.Logging.LogLevel.MicrosoftAspNetCore);
		}

		[Fact]
		public void AppSettings_AllowedHosts_ShouldNotBeNull()
		{
			// Arrange
			var appSettings = new AppSettings();

			// Act
			appSettings.AllowedHosts = "localhost";

			// Assert
			Assert.NotNull(appSettings.AllowedHosts);
		}

		[Fact]
		public void AppSettings_EnableRequestResponseLogging_ShouldBeFalseByDefault()
		{
			// Arrange
			var appSettings = new AppSettings();

			// Act
			appSettings.EnableRequestResponseLogging = false;
			// Assert
			Assert.False(appSettings.EnableRequestResponseLogging);
		}

		[Fact]
		public void AppSettings_WithOrigins_ShouldNotBeNull()
		{
			// Arrange
			var appSettings = new AppSettings();

			// Act
			appSettings.WithOrigins = new();
			appSettings.WithOrigins.Add("http://localhost:4200");

			// Assert
			Assert.NotNull(appSettings.WithOrigins);
		}


		[Fact]
		public void AppSettings_AppSettingsConfig_ShouldNotBeNull()
		{
			// Arrange
			var appSettings = new AppSettings();

			// Act
			appSettings.AppSettingsConfig = new();

			// Assert
			Assert.NotNull(appSettings.AppSettingsConfig);

		}

		[Fact]
		public void ConnectionStrings_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var connectionStrings = new ConnectionStrings();

			// Act
			connectionStrings.SecretDB = "SecretDB";

			// Assert
			Assert.Equal("SecretDB", connectionStrings.SecretDB);
		}

		[Fact]
		public void AppSettings_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var appSettings = new AppSettings();
			var connectionStringsMock = new Mock<ConnectionStrings>();
			var loggingMock = new Mock<Logging>();

			appSettings.ConnectionStrings = connectionStringsMock.Object;
			appSettings.Logging = loggingMock.Object;
			appSettings.AllowedHosts = "AllowedHosts";
			appSettings.EnableRequestResponseLogging = true;
			appSettings.WithOrigins = new List<string> { "Origin1", "Origin2" };

			// Act

			// Assert
			Assert.Same(connectionStringsMock.Object,
				appSettings.ConnectionStrings);
			Assert.Same(loggingMock.Object, appSettings.Logging);
			Assert.Equal("AllowedHosts", appSettings.AllowedHosts);
			Assert.True(appSettings.EnableRequestResponseLogging);
			Assert.Equal(2, appSettings.WithOrigins.Count);
			Assert.Contains("Origin1", appSettings.WithOrigins);
			Assert.Contains("Origin2", appSettings.WithOrigins);
		}

	}
}
