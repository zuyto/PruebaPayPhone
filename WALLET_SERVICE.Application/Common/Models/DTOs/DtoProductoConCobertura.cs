// <copyright file="DtoProductoConCobertura.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoProductoConCobertura
	{


		public int IdGrupo { get; set; }

		//PRODUCTOS

		public long PrdLvlChild { get; set; }

		public string PrdLvlNumber { get; set; } = null!;

		public long? OrigenNumber { get; set; }

		public long? OrgLvlChild { get; set; }

		public byte? IdTipoNodo { get; set; }

		public int IdNodo { get; set; }

		public int? IdValorAtributo { get; set; }
		public int CantidadSku { get; set; }



		//COBERTURA
		public int IdRedZona { get; set; }

		public int? IdZona { get; set; }

		public int? IdCiudad { get; set; }

		public int? IdRed { get; set; }

		public int? IdDepto { get; set; }

		public int IdFlujo { get; set; }

		public string? Sigla { get; set; }

		public int? IdPromesaCliente { get; set; }

		public string? Promesa { get; set; }

		public string? ValorAtributo { get; set; }

		public int? IdCanal { get; set; }

		public int? IdTipoNodoInicial { get; set; }

		public string? CodigoInternoInicial { get; set; }

		public long? NumberInternoInicial { get; set; }

		public int? IdTipoNodoFinal { get; set; }

		public string? CodigoInternoFinal { get; set; }

		public long? NumberInternoFinal { get; set; }


	}
}
