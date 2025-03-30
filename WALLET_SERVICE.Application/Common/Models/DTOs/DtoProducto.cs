// <copyright file="DtoProducto.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoProducto
	{
		public string? RefProducto { get; set; }
		public string? Descripcion { get; set; }
		public string? Volumen { get; set; }
		public string? Peso { get; set; }
		public string? Alto { get; set; }
		public string? Ancho { get; set; }
		public string? Profundidad { get; set; }
		public string? Apilable { get; set; }
		public string? Codigo { get; set; }
		public string? RotacionesPermitidas { get; set; }
		public string? Linea { get; set; }
		public string? SubLinea { get; set; }
	}
}
