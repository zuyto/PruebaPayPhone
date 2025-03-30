// <copyright file="SerilogImplements.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Logger;
using WALLET_SERVICE.Logger.Models;

namespace WALLET_SERVICE.Application.Services.Serilog
{
	public class SerilogImplements : ISerilogImplements
	{
		#region private members

		private readonly ILogger<SerilogImplements> _logger;

		#endregion

		#region constructors

		public SerilogImplements(ILogger<SerilogImplements> logger)
		{
			_logger = logger;
		}

		#endregion

		#region public methods
		/// <summary>
		/// Metodo para procesar los metodos por defecto de serilog WALLET_SERVICE
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="method"></param>
		/// <param name="parameters"></param>
		/// <param name="message"></param>
		/// <param name="memberName"></param>
		/// <param name="sourceFilePath"></param>
		/// <param name="sourceLineNumber"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public string? ObtainMessageDefault(string messageType, string? method,
			string? parameters, string? message, [CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{

			var logErrorOptions = new LogErrorOptions
			{
				MemberName = memberName,
				SourceFilePath = sourceFilePath,
				SourceLineNumber = sourceLineNumber
			};

			switch (messageType)
			{
				case ConfigurationMessageType.Error:
					logErrorOptions.Message = message;
					logErrorOptions.EventId = new EventId(1);
					logErrorOptions.Args = new[] { ConfigurationMessageType.Error.ToUpper(), message, method, parameters };
					_logger.LogError(logErrorOptions);
					break;
				case ConfigurationMessageType.Critical:
					logErrorOptions.Message = message;
					logErrorOptions.EventId = new EventId(2);
					logErrorOptions.Args = new[] { ConfigurationMessageType.Critical.ToUpper(), message, method, parameters };
					_logger.LogCritical(logErrorOptions);
					break;
				case ConfigurationMessageType.Warning:
					logErrorOptions.Message = message;
					logErrorOptions.EventId = new EventId(3);
					logErrorOptions.Args = new[] { ConfigurationMessageType.Warning.ToUpper(), message, method, parameters };
					_logger.LogWarning(logErrorOptions);
					break;
				case ConfigurationMessageType.Information:
					logErrorOptions.Message = message;
					logErrorOptions.EventId = new EventId(4);
					logErrorOptions.Args = new[] { ConfigurationMessageType.Information.ToUpper(), message, method, parameters };
					_logger.LogInformation(logErrorOptions);
					break;
			}

			return message;
		}

		#endregion
	}
}
