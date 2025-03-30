// <copyright file="SwaggerExtension.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace WALLET_SERVICE.Api.Middleware
{
	/// <summary>
	///     Configuración de Swagger File
	/// </summary>
	/// 
	[ExcludeFromCodeCoverage]
	public static class SwaggerExtension
	{
		/// <summary>
		///     Agrega la configuración de Swagger
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddSwagger(
			this IServiceCollection services)
		{
			services
				.AddTransient<IConfigureOptions<SwaggerGenOptions>,
					ConfigureSwaggerOptions>();
			services.AddSwaggerGen(c =>
			{
				// Set the comments path for the Swagger JSON and UI.
				c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
			});
			return services;
		}
	}
}
