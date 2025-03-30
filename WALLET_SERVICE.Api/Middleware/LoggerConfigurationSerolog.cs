// <copyright file="LoggerConfigurationSerolog.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using Serilog;
using Serilog.Events;

namespace WALLET_SERVICE.Api.Middleware
{
	/// <summary>
	///     Clase para la configuración de Serilog
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class LoggerConfigurationSerolog
	{
		/// <summary>
		///     Metodo para agregar configuración de Serilog
		/// </summary>
		/// <returns></returns>
		public static Serilog.Core.Logger Add()
		{
			return new LoggerConfiguration()
			.WriteTo.Console(LogEventLevel.Error)
			.Enrich.FromLogContext()
			.CreateLogger();
		}
	}
}
