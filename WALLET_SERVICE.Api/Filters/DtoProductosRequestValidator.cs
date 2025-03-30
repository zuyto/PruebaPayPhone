// <copyright file="DtoProductosRequestValidator.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using WALLET_SERVICE.Application.Common.Models.DTOs;

namespace WALLET_SERVICE.Api.Filters
{
	/// <summary>
	/// 
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class DtoProductosRequestValidator : AbstractValidator<DtoProductosRequestCont>
	{
		/// <summary>
		/// 
		/// </summary>
		public DtoProductosRequestValidator()
		{

			RuleFor(x => x.Sku)
			.NotEmpty().WithMessage("Sku no puede estar vacío.")
			.NotNull().WithMessage("Sku no puede estar null");

			RuleFor(x => x.Cantidad)
				.NotEmpty().WithMessage("Cantidad no puede estar vacía.")
				.NotNull().WithMessage("Cantidad no puede estar null");


		}
	}
}
