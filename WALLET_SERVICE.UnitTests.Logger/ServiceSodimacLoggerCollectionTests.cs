// <copyright file="ServiceSodimacLoggerCollectionTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog.Extensions.Logging;

using WALLET_SERVICE.Logger;
using WALLET_SERVICE.Logger.Models;

namespace WALLET_SERVICE.UnitTests.Logger
{
	public class ServiceLoggerCollectionTests
	{
		[Fact]
		public void AddLogger_ShouldConfigureLoggerWithDefaultSettings()
		{
			// Arrange
			var services = new ServiceCollection();
			var setupAction = new LoggerOptions();

			// Act
			var result = services.AddLogger(setupAction);

			// Assert
			Assert.Same(setupAction, result);
			var serviceProvider = services.BuildServiceProvider();
			var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
			var logger = loggerFactory?.CreateLogger("Test");
			Assert.NotNull(logger);
			Assert.IsType<SerilogLoggerFactory>(loggerFactory);
		}

		[Fact]
		public void AddLogger_ShouldConfigureLoggerWithCustomProperties()
		{
			// Arrange
			var services = new ServiceCollection();
			var setupAction = new LoggerOptions();
			var customProperties = new Dictionary<string, object>
			{
				{ "CustomProperty1", "Value1" },
				{ "CustomProperty2", 123 }
			};

			// Act
			var result = services.AddLogger(setupAction, customProperties);

			// Assert
			Assert.Same(setupAction, result);
			var serviceProvider = services.BuildServiceProvider();
			var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
			var logger = loggerFactory?.CreateLogger("Test");
			Assert.NotNull(logger);
			Assert.IsType<SerilogLoggerFactory>(loggerFactory);
		}
		[Fact]
		public void IncludeProperty_ShouldAddPropertyToLoggerConfiguration()
		{
			// Arrange
			var services = new ServiceCollection();
			var setupAction = new LoggerOptions();
			services.AddLogger(setupAction);
			var serviceProvider = services.BuildServiceProvider();
			var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
			var logger = loggerFactory?.CreateLogger("Test");

			// Act
			setupAction.IncludeProperty("TestProperty", "TestValue");

			// Assert
			Assert.NotNull(logger);
			Assert.IsType<SerilogLoggerFactory>(loggerFactory);
		}
	}

}
