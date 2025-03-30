// <copyright file="DtoGenericResponse.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase
{
	/// <summary>
	/// Base class for generic response
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class DtoGenericResponse<T>
	{
		/// <summary>
		/// Mensaje del error a mostrar
		/// </summary>
		public string? Mensaje { get; set; }
		/// <summary>
		/// Muestra si la respuesta fue exitosa {True} o fallida {False}
		/// </summary>
		public bool EsExitoso { get; set; }
		/// <summary>
		/// Muestra el resultado o response de la consulta a la DB
		/// </summary>
		public T? Resultado { get; set; }

		public bool IsOkEsquema { get; set; }
	}
}
