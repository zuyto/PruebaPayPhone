// <copyright file="ConfigureSwaggerOptions.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Api.Middleware
{
	/// <summary>
	///     Configuración de Swagger
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _provider;

		/// <summary>
		///     Contructor
		/// </summary>
		/// <param name="provider"></param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
		{
			_provider = provider;
		}


		//In C# the tag ---> <inheritdoc/>
		//states that a documentation comment must inherit
		//documentation from a base class or implemented interface.
		/// <inheritdoc />
		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description in _provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName,
					CreateInfoForApiVersion(description));
			}
		}

		private static OpenApiInfo CreateInfoForApiVersion(
			ApiVersionDescription description)
		{
			var dotNetVersion = Environment.Version.ToString();

			var info = new OpenApiInfo
			{
				Version = $"v{description.ApiVersion} - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}",
				Title = Resources.Title,
				Description = $"{Resources.Description} - NetCore: {dotNetVersion}",
				TermsOfService =
					new Uri(ConfigurationStruct.TermsOfServices),
				Contact =
					new OpenApiContact
					{
						Name = "Mauro Martinez",
						Url = new Uri(ConfigurationStruct.Contact)
					},
				License = new OpenApiLicense
				{
					Name = "Apache License, Version 2.0",
					Url = new Uri(ConfigurationStruct.License)
				}
			};
			if (description.IsDeprecated)
			{
				info.Description += "This API version has been deprecated.";
			}

			return info;
		}
	}
}
