// <copyright file="ValidationFilter.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using WALLET_SERVICE.Api.Response;
using WALLET_SERVICE.Application.Common.Static;

namespace WALLET_SERVICE.Api.Filters
{
	/// <summary>
	///     Filtro para las validaciones
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ValidationFilter : IAsyncActionFilter
	{
		/// <summary>
		///     OnActionExecutionAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="next"></param>
		/// <returns></returns>
		public async Task OnActionExecutionAsync(ActionExecutingContext context,
			ActionExecutionDelegate next)
		{
			if (!context.ModelState.IsValid)
			{
				var errorResponse = ApiResponse<ModelStateDictionary>.CreateUnsuccessful(context.ModelState, UserTypeMessages.ERROR_REQUEST);
				context.Result = new BadRequestObjectResult(errorResponse);
				return;
			}

			await next();
		}
	}

	/// <summary>
	///     FluentValidationFilter
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class FluentValidationFilter : IAsyncActionFilter
	{
		/// <summary>
		///     OnActionExecutionAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="next"></param>
		/// <returns></returns>
		public async Task OnActionExecutionAsync(ActionExecutingContext context,
			ActionExecutionDelegate next)
		{
			var errorList = new List<string>();
			foreach (var arg in context.ActionArguments.Select(arg => arg.Value))
			{
				// Omitir valores nulos
				if (arg == null)
				{
					continue;
				}

				var vt = typeof(AbstractValidator<>);
				var et = arg.GetType();
				var evt = vt.MakeGenericType(et);
				var validatorType = FindValidatorType(evt);
				// Omitir si no tiene validador
				if (validatorType == null)
				{
					continue;
				}

				var validatorInstance =
					Activator.CreateInstance(validatorType) as IValidator;
				var contextV = new ValidationContext<object>(arg);
				var result = validatorInstance != null
					? await validatorInstance.ValidateAsync(contextV)
					: null;
				// Si hay errores copiarlos en una lista
				if (result != null && !result.IsValid)
				{
					foreach (var e in result.Errors)
					{
						errorList.Add(e.ErrorMessage);
					}
				}
			}

			// Si hay errores crea la respuesta personalizada
			if (errorList.Count > 0)
			{
				var errorResponse = ApiResponse<List<string>>.CreateUnsuccessful(errorList, UserTypeMessages.ERROR_REQUEST);
				context.Result = new BadRequestObjectResult(errorResponse);
				return;
			}

			await next();
		}

		/// <summary>
		///     FindValidatorType
		/// </summary>
		/// <param name="evt"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static Type? FindValidatorType(Type evt)
		{
			if (evt == null)
			{
				ArgumentNullException.ThrowIfNull(evt);
			}

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var myAssen = Assembly.GetExecutingAssembly();
			var names = myAssen.FullName != null && myAssen.FullName.Contains('.')
				? myAssen.FullName.Split(".")[0]
				: string.Empty;

			foreach (var assembly in assemblies.Where(x =>
						 x.FullName != null && x.FullName.Contains(names)))
			{
				foreach (var type in assembly.GetTypes())
				{
					if (type.IsSubclassOf(evt) && !type.IsAbstract)
					{
						return type;
					}
				}
			}

			return null;
		}
	}
}
