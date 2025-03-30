// <copyright file="SwaggerExtensionTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

using WALLET_SERVICE.Api.Middleware;

namespace WALLET_SERVICE.UnitTests.Api.Api.UnitTest.Middleware
{
	public class SwaggerExtensionTests
	{
		[Fact]
		public void AddSwagger_ShouldRegisterSwaggerGenOptions()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddSwagger();
			var serviceProvider = services.BuildServiceProvider();

			// Assert
			var options = serviceProvider.GetService<IConfigureOptions<SwaggerGenOptions>>();
			Assert.NotNull(options);
		}

		[Fact]
		public void AddSwagger_ShouldRegisterConfigureSwaggerOptions()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddSwagger();
			var serviceProvider = services.BuildServiceProvider();

			// Assert
			var configureOptions = serviceProvider.GetService<IConfigureOptions<SwaggerGenOptions>>();
			Assert.NotNull(configureOptions);
			Assert.IsType<ConfigureNamedOptions<SwaggerGenOptions>>(configureOptions);
		}
	}
}
