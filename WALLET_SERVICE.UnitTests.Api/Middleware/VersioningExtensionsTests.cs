// <copyright file="VersioningExtensionsTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using WALLET_SERVICE.Api.Middleware;

namespace WALLET_SERVICE.UnitTests.Api.Api.UnitTest.Middleware
{
	public class VersioningExtensionsTests
	{
		[Fact]
		public void AddVersioning_ShouldConfigureApiVersioning()
		{
			// Arrange
			var builder = WebApplication.CreateBuilder();
			Environment.SetEnvironmentVariable("SwaggerMajorVersion", "1");
			Environment.SetEnvironmentVariable("SwaggerMinorVersion", "0");
			var services = builder.Services;
			services.AddLogging(); // Add logging service

			// Act
			builder.AddVersioning();
			var serviceProvider = services.BuildServiceProvider();
			var apiVersioningOptions = serviceProvider.GetService<IOptions<ApiVersioningOptions>>()?.Value;

			// Assert
			Assert.NotNull(apiVersioningOptions);
			Assert.Equal(new ApiVersion(1, 0), apiVersioningOptions.DefaultApiVersion);
			Assert.True(apiVersioningOptions.AssumeDefaultVersionWhenUnspecified);
			Assert.True(apiVersioningOptions.ReportApiVersions);
			Assert.IsType<HeaderApiVersionReader>(apiVersioningOptions.ApiVersionReader);
		}

		[Fact]
		public void AddVersioning_ShouldConfigureApiExplorerOptions()
		{
			// Arrange
			var builder = WebApplication.CreateBuilder();
			var services = builder.Services;
			services.AddLogging(); // Add logging service

			// Act
			builder.AddVersioning();
			var serviceProvider = services.BuildServiceProvider();
			var apiExplorerOptions = serviceProvider.GetService<IApiVersionDescriptionProvider>();

			// Assert
			Assert.NotNull(apiExplorerOptions);
			var options = serviceProvider.GetService<IOptions<ApiExplorerOptions>>()?.Value;
			Assert.NotNull(options);
			Assert.Equal("'v'VVV", options.GroupNameFormat);
			Assert.True(options.SubstituteApiVersionInUrl);
		}
	}
}
