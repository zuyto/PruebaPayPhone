// <copyright file="DtoGenericResponseHttp.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase
{
	/// <summary>
	/// Base class for generic response HTTP
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class DtoGenericResponseHttp : DtoGenericResponse<object>
	{
		/// <summary>
		/// Muestra el resultado de un Endpoint Serializado
		/// </summary>
		public string? ResultadoHttp { get; set; }

	}
}
