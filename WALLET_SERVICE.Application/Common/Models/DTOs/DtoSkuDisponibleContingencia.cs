// <copyright file="DtoSkuDisponibleContingencia.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoSkuDisponibleContingencia
	{
		public string? Sku { get; set; }
		public string? Nodo { get; set; }
		public string? Atributo { get; set; }
		public string? Zona { get; set; }
		public string? Promesa { get; set; }
		public string? Fecha { get; set; }
	}
}
