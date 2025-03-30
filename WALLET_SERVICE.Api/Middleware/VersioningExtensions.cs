// <copyright file="VersioningExtensions.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Asp.Versioning;

namespace WALLET_SERVICE.Api.Middleware
{
	/// <summary>
	///     Versioning Extensions
	/// </summary>
	public static class VersioningExtensions
	{
		/// <summary>
		///     Add Versioning
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static WebApplicationBuilder AddVersioning(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddApiVersioning(o =>
			{
				o.DefaultApiVersion = new ApiVersion(Convert.ToInt32(Environment.GetEnvironmentVariable("SwaggerMajorVersion")), Convert.ToInt32(Environment.GetEnvironmentVariable("SwaggerMinorVersion")));
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.ReportApiVersions = true;
				o.ApiVersionReader = new HeaderApiVersionReader();
			}).AddApiExplorer(options =>
			{
				//semantic versioning
				//first character is the principal or greater version
				//second character is the minor version
				//third character is the patch
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
			});
			return builder;
		}
	}
}
