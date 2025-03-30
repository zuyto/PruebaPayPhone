// <copyright file="DtoResultadoAnalisisCont.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using WALLET_SERVICE.Application.Common.Enumerations;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoResultadoAnalisisCont
	{
		public TipoResultadoCont Tipo { get; set; }
		public DtoGrupoProductosCont? GrupoSeleccionado { get; set; }
	}
}
