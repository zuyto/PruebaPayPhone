// <copyright file="DtoDatosRequestValidator.cs" company="Mauro Martinez">
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
	///
	[ExcludeFromCodeCoverage]
	public class DtoDatosRequestValidator : AbstractValidator<DtoTransferJsonRequest>
	{
		/// <summary>
		/// 
		/// </summary>
		public DtoDatosRequestValidator()
		{
			RuleFor(x => x.FromWalletId)
			.GreaterThan(0).WithMessage("El FromWalletId debe ser un número positivo.");

			RuleFor(x => x.ToWalletId)
				.GreaterThan(0).WithMessage("El ToWalletId debe ser un número positivo.")
				.NotEqual(x => x.FromWalletId).WithMessage("El FromWalletId y el ToWalletId no pueden ser iguales.");

			RuleFor(x => x.Amount)
				.GreaterThan(0).WithMessage("El Amount debe ser mayor a 0.");

		}
	}
}
