// <copyright file="DtoBrokerHeader.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase
{
	[ExcludeFromCodeCoverage]
	public class DtoBrokerHeader
	{
		public string? channel { get; set; }
		public string? terminalId { get; set; }
		public string? service { get; set; }
	}
}
