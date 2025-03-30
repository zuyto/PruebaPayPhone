// <copyright file="NullBodyRequestMiddleware.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using WALLET_SERVICE.Api.Response;
using WALLET_SERVICE.Application.Common.Static;

namespace WALLET_SERVICE.Api.Middleware
{
	/// <summary>
	/// 
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class NullBodyRequestMiddleware
	{
		private readonly RequestDelegate _next;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="next"></param>
		public NullBodyRequestMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			// Si el método de la solicitud es POST o PUT y el cuerpo es nulo
			if ((context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put || context.Request.Method == HttpMethods.Delete) && context.Request.ContentLength == 0)
			{

				var errorResponse = ApiResponse<List<string>>.CreateUnsuccessful(new List<string> { UserTypeMessages.NULL_BODY_REQUEST }, UserTypeMessages.ERROR_REQUEST);
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				await context.Response.WriteAsJsonAsync(errorResponse);
				return;
			}

			// Continúa hacia el siguiente middleware
			await _next(context);
		}
	}

}
