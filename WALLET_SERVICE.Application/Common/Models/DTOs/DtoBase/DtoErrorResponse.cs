// <copyright file="DtoErrorResponse.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase
{
	/// <summary>
	/// Base class for error response
	/// </summary>
	[ExcludeFromCodeCoverage]

	public class DtoErrorResponse
	{
		public string? TipoExcepcion { get; set; }
		public string? Mensaje { get; set; }
		public string? Objeto { get; set; }
		public string? Metodo { get; set; }
		public string? Stacktrace { get; set; }
		public string? DetalleInnerException { get; set; }
	}
}
