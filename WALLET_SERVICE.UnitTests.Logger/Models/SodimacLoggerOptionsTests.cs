// <copyright file="SodimacLoggerOptionsTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Logger.Models;

namespace WALLET_SERVICE.UnitTests.Logger.Models
{
	public class LoggerOptionsTests
	{
		[Fact]
		public void LoggerOptions_PropertiesInitializedCorrectly()
		{
			// Arrange
			var LoggerOptions = new LoggerOptions();

			// Act

			// Assert
			Assert.Equal("v1.1", LoggerOptions.Meta);
			Assert.Equal(string.Empty, LoggerOptions.Autor);
			Assert.Equal(string.Empty, LoggerOptions.Area);
			Assert.Equal(string.Empty, LoggerOptions.Aplicacion);
			Assert.Equal(string.Empty, LoggerOptions.Proceso);
			Assert.Equal(string.Empty, LoggerOptions.Servicio);
			Assert.Equal(string.Empty, LoggerOptions.Endpoint);
		}
	}
}
