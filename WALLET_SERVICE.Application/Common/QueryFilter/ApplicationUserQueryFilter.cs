// <copyright file="ApplicationUserQueryFilter.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.QueryFilter
{
	public class ApplicationUserQueryFilter
	{
		public int IdAplicacion { get; set; }

		public DateTime? FechaInicio { get; set; }

		public DateTime? FechaFin { get; set; }
	}
}
