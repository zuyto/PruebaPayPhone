// <copyright file="DtoFechasResponseCont.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoFechasResponseCont
	{
		public List<DtoFechaCont>? Fechas { get; set; }
		public List<DtoFechaCont>? FechasDisponibles { get; set; }

	}
}
