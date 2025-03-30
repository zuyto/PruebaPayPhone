// <copyright file="AppSettingsTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Moq;

using WALLET_SERVICE.Domain;

namespace WALLET_SERVICE.UnitTests.Api
{
	/// <summary>
	///     AppSettingsTests
	/// </summary>
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
		public void AppSettings_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var appSettings = new AppSettings();
			var connectionStringsMock = new Mock<ConnectionStrings>();
			var loggingMock = new Mock<Logging>();
			var appSettingsConfigMock = new Mock<AppSettingsConfig>();

			appSettings.ConnectionStrings = connectionStringsMock.Object;
			appSettings.Logging = loggingMock.Object;
			appSettings.AllowedHosts = "AllowedHosts";
			appSettings.EnableRequestResponseLogging = true;
			appSettings.WithOrigins = new List<string> { "Origin1", "Origin2" };
			appSettings.AppSettingsConfig = appSettingsConfigMock.Object;

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
			Assert.Same(appSettingsConfigMock.Object, appSettings.AppSettingsConfig);

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
	}
}
