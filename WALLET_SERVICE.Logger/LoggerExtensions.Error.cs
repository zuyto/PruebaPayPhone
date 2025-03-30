// <copyright file="LoggerExtensions.Error.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.Extensions.Logging;

using WALLET_SERVICE.Logger.Models;
using WALLET_SERVICE.Logger.Static;

namespace WALLET_SERVICE.Logger
{
	public static class LoggerExtensions
	{
		public static Dictionary<string, object> MethodsProperties(string memberName, string sourceFilePath, int sourceLineNumber, LogLevel level)
		{
			var lvl = Enum.GetName(typeof(LogLevel), level) ?? ConfigTypeMessage.LVL;

			// Asegurarse de que los datos del archivo y el número de línea no contienen información sensible o controlada por el usuario.
			return new Dictionary<string, object>
			{
				[ConfigTypeMessage.FECHA] = DateTime.Now.ToString(ConfigTypeMessage.FORMATDATE),
				[ConfigTypeMessage.HORA] = DateTime.Now.ToString(ConfigTypeMessage.FORMATHOUR),
				[ConfigTypeMessage.NIVEL] = lvl,
				[ConfigTypeMessage.METODO] = memberName,
				[ConfigTypeMessage.ARCHIVO] = sourceFilePath,
				[ConfigTypeMessage.LINEA] = sourceLineNumber
			};
		}

		public static void LogError(this ILogger logger, LogErrorOptions options)
		{
			LogWithOptions(logger, LogLevel.Error, options);
		}

		public static void LogCritical(this ILogger logger, LogErrorOptions options)
		{
			LogWithOptions(logger, LogLevel.Critical, options);
		}

		public static void LogInformation(this ILogger logger, LogErrorOptions options)
		{
			LogWithOptions(logger, LogLevel.Information, options);
		}

		public static void LogWarning(this ILogger logger, LogErrorOptions options)
		{
			LogWithOptions(logger, LogLevel.Warning, options);
		}

		public static void LogWithOptions(ILogger logger, LogLevel level, LogErrorOptions options)
		{
			using (logger.BeginScope(MethodsProperties(options.MemberName, options.SourceFilePath, options.SourceLineNumber, level)))
			{
				var obj = options.Args ?? [];
				logger.Log(level, options.EventId, options.Exception, ConfigTypeMessage.GENERAL01, obj);
			}
		}

	}
}
