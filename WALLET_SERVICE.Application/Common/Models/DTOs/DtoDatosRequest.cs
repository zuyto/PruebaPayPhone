// <copyright file="DtoDatosRequest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoDatosRequest
	{
		public int IdZona { get; set; }
		public int IdCiudad { get; set; }
		public int IdDepartamento { get; set; }
		public int IdCanal { get; set; }
		public int OrgLvlChild { get; set; }
	}
}
