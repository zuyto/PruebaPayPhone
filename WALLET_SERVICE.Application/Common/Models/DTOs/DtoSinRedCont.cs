// <copyright file="DtoSinRedCont.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs
{
	[ExcludeFromCodeCoverage]
	public class DtoSinRedCont
	{
		public string? IdGrupo { get; set; }
		public List<DtoProductoCont>? Productos { get; set; }
	}
}
