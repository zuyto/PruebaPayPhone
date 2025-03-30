// <copyright file="LogErrorSodimacOptions.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.Extensions.Logging;

using WALLET_SERVICE.Logger.Static;

namespace WALLET_SERVICE.Logger.Models
{
	public class LogErrorOptions
	{
		public EventId EventId { get; set; }
		public Exception? Exception { get; set; }
		public string? Message { get; set; } = ConfigTypeMessage.LOGERGUION;
		public object?[]? Args { get; set; }
		public string MemberName { get; set; } = string.Empty;
		public string SourceFilePath { get; set; } = string.Empty;
		public int SourceLineNumber { get; set; }
	}
}
