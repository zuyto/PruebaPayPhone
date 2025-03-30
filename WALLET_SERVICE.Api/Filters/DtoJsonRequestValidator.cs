// <copyright file="DtoJsonRequestValidator.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

//using FluentValidation;
//using System.Diagnostics.CodeAnalysis;
//using WALLET_SERVICE.Application.Common.Models.DTOs;

//namespace WALLET_SERVICE.Api.Filters
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [ExcludeFromCodeCoverage]
//    public class DtoTransferJsonRequestValidator : AbstractValidator<DtoTransferJsonRequest>
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public DtoTransferJsonRequestValidator()
//        {

//            RuleFor(x => x.Datos)
//                .NotNull().WithMessage("Los datos no pueden ser nulos.")
//            .SetValidator(new DtoDatosRequestValidator()!);

//            RuleFor(x => x.Productos)
//            .NotNull().WithMessage("La lista de productos no puede ser nula.")
//            .NotEmpty().WithMessage("La lista de productos no puede estar vacía.")
//            .ForEach(producto => producto.SetValidator(new DtoProductosRequestValidator()));

//        }
//    }
//}
