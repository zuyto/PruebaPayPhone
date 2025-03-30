// <copyright file="DependecyInjectionTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using AspNetCoreRateLimit;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Domain;
using WALLET_SERVICE.Infrastructure;
using WALLET_SERVICE.Infrastructure.Context;
using WALLET_SERVICE.Logger.Models;

namespace WALLET_SERVICE.UnitTests.Infrastructure
{
	public class DependecyInjectionTests
	{
		[Fact]
		public void ConfigureRateLimitiong_ShouldConfigureRateLimitOptions()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			DependecyInjection.ConfigureRateLimitiong(services);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<IOptions<IpRateLimitOptions>>().Value;

			Assert.Equal(ConfigurationStruct.EnableEndpointRateLimiting, options.EnableEndpointRateLimiting);
			Assert.Equal(ConfigurationStruct.StackBlockedRequests, options.StackBlockedRequests);
			Assert.Equal(429, options.HttpStatusCode);
			Assert.Equal(ConfigurationStruct.RealIpHeader, options.RealIpHeader);
			Assert.Single(options.GeneralRules);
			Assert.Equal(ConfigurationStruct.Endpoint, options.GeneralRules[0].Endpoint);
			Assert.Equal(ConfigurationStruct.Period, options.GeneralRules[0].Period);
			Assert.Equal(100000, options.GeneralRules[0].Limit);
		}
		[Fact]
		public void ConfigureRateLimiting_RegistersExpectedServices()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			DependecyInjection.ConfigureRateLimitiong(services); // Asumiendo que este método está definido y accesible

			// Assert
			// Verificar que los servicios específicos están registrados
			Assert.Contains(services, service => service.ServiceType == typeof(IMemoryCache));
			Assert.Contains(services, service => service.ServiceType == typeof(IRateLimitConfiguration));
		}

		[Fact]
		public void AddInfrastructure_ShouldConfigureCorsPolicy()
		{
			// Arrange
			var builder = WebApplication.CreateBuilder();
			var services = builder.Services;
			Environment.SetEnvironmentVariable(ConfigurationStruct.WithOrigins, "http://example.com");

			// Act
			builder.AddInfrastructure();

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;
			Assert.NotNull(corsOptions);
			var corsPolicy = corsOptions.GetPolicy(ConfigurationStruct.CorsPolicy);
			Assert.NotNull(corsPolicy);
			Assert.Contains(corsPolicy.Origins, origin => origin == "http://example.com");
		}

		[Fact]
		public void AddInfrastructure_ShouldConfigureLogger()
		{
			// Arrange
			var builder = WebApplication.CreateBuilder();
			var services = builder.Services;
			Environment.SetEnvironmentVariable(ConfigurationStruct.Gerencia, "GerenciaTest");
			Environment.SetEnvironmentVariable(ConfigurationStruct.Celula, "CelulaTest");
			Environment.SetEnvironmentVariable(ConfigurationStruct.Aplicacion, "AplicacionTest");
			Environment.SetEnvironmentVariable(ConfigurationStruct.Proyecto, "ProyectoTest");

			// Act
			builder.AddInfrastructure();

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var loggerOptions = serviceProvider.GetRequiredService<IOptions<LoggerOptions>>().Value;
			Assert.NotNull(loggerOptions);
			Assert.Equal("v1.1", loggerOptions.Meta);
			Assert.Equal("", loggerOptions.Autor);
			Assert.Equal("", loggerOptions.Area);
			Assert.Equal("", loggerOptions.Aplicacion);
			Assert.Equal("", loggerOptions.Proceso);
			Assert.Equal("", loggerOptions.Servicio);
			Assert.Equal("", loggerOptions.Endpoint);
		}

		[Fact]
		public void AddDbContext_ShouldUseOracleWithDecryptedSecret()
		{
			// Arrange
			var builder = WebApplication.CreateBuilder();
			var services = builder.Services;
			var secretDb = "IbCquloeG5C2zpO2xuP4NFnNci64F6pCaZvEuZ+8jkH0ihUF9kZxHij44FuHe0+Y8QmZDDIzirFBwpXBq4IB7E1L75kK5qYiYgtPD4uNLMZSdeu99x4LcbVcAlMQRhtVEFoUvHEpzUDFhJvof829EuOnhPpKUNBnKwEKG7U0wELgFCP3S/cH+1tI4gnD2MXIwsbla7Q4LfPIN+0BSyY/fuY6nlrlbuzBSBAYfnf3+doOzQSAt7xbCEoT7RyfwCZGl4FDb1SvJO6/AUMqt94ZracEqa9AdHrae/RcQOoNYio=";
			var PROD_SGL = "VTJGc2RHVmtYMThBQUFBQUFBQUFBSnpZREtmdHMwZCswTjdDWEVPSUl1S2h2b2ZJM05kTWZFTDNPdlE2MExBQlN4VkRRS3lsaVFZd0F6MVRia2J2Yy82WkFjVEFGblkxeTRpaUFmalcrbmY5dGhqMll2WkQ1L0hZeVY3Z212RFQwSmdqRk1RSnVZRmxNTXg0QVdkUmdOZEVLS2FRN1BCZE5kQlo0SFdOL3hhNEtPS3RkRmV2WlRSVEhsT2UwRTVHb25sODN5ckFhb3hnTk9YbXdOQUE5ZUJDV0pkZGhTcERYeWRGeEpxVlkrVHFSdlFIeENmZzZaMEp0NjcvZk1mdkd5Y1lEQ1RTU0JITTVLUHc0ME50R1p3OG9LL04rTzB5N3k1QyswOE8rdVE9";
			Environment.SetEnvironmentVariable(ConfigurationStruct.DbSecretDB, secretDb);
			Environment.SetEnvironmentVariable(ConfigurationStruct.PROD_SGL, PROD_SGL);

			// Act
			builder.AddDbContext();

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var context = serviceProvider.GetService<DynamicContext>();
			Assert.NotNull(context);
		}

		[Fact]
		public void AddInfrastructure_ShouldConfigureServicesCorrectly()
		{
			// Arrange
			var builderMock = WebApplication.CreateBuilder();
			var services = builderMock.Services;


			// Act
			var result = DependecyInjection.AddInfrastructure(builderMock);

			// Assert
			Assert.NotNull(result);
			Assert.Contains(services, s => s.ServiceType == typeof(IUnitOfWorkDynamic));
			Assert.Contains(services, s => s.ServiceType == typeof(IUnitOfWorkWalletTransfer));
		}

		[Fact]
		public void ConfigureRateLimitiong_ShouldConfigureRateLimitingCorrectly()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			DependecyInjection.ConfigureRateLimitiong(services);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var rateLimitConfig = serviceProvider.GetService<IRateLimitConfiguration>();
			Assert.NotNull(rateLimitConfig);
		}

		[Fact]
		public void ConfigureRateLimitiong_ShouldThrowException_WhenServicesIsNull()
		{
			// Arrange
			ServiceCollection? services = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => DependecyInjection.ConfigureRateLimitiong(services!));
		}

		[Fact]
		public void ConfigureAppSettingsManagerConnections_ShouldConfigureAppSettingsCorrectly()
		{
			// Arrange
			var builderMock = WebApplication.CreateBuilder();
			var services = builderMock.Services;

			Environment.SetEnvironmentVariable(ConfigurationStruct.HCAV_SGL, "TestConnectionString");
			Environment.SetEnvironmentVariable(ConfigurationStruct.PROD_SGL, "ProdConnectionString");

			// Act
			DependecyInjection.AddInfrastructure(builderMock);
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetService<IOptions<AppSettingsConfig>>();

			// Assert
			Assert.NotNull(options);
			Assert.Equal("TestConnectionString", options.Value.CadenaConexion);
			Assert.Equal("TestConnectionString", options.Value.HCAV);
			Assert.Equal("ProdConnectionString", options.Value.CadenaConexionTag);
			Assert.Equal("ProdConnectionString", options.Value.PROD);
		}

	}
}
