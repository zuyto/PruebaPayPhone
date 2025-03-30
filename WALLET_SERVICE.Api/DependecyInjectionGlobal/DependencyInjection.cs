// <copyright file="DependencyInjection.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Net.Security;

using Newtonsoft.Json.Serialization;

using WALLET_SERVICE.Api.Filters;
using WALLET_SERVICE.Api.Middleware;
using WALLET_SERVICE.Application;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Infrastructure;

namespace WALLET_SERVICE.Api.DependecyInjectionGlobal
{
	/// <summary>
	/// Clase encargada de realizar la inyeccion de dependencias
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class DependencyInjection
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
		{

			builder?.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			}).ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
			builder?.Services.AddControllersWithViews(option =>
			{
				option.Filters.Add(new ValidationFilter());
				option.Filters.Add(new FluentValidationFilter());
				////option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; // Permite recibir el request en null
			});
			builder?.Services.ConfigureRateLimitiong();


			builder?.Services.AddProblemDetails();
			builder?.Services.AddHealthChecks();
			builder?.AddDbContext();
			builder?.AddInfrastructure();
			builder?.Services.AddApplication();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder?.Services.AddEndpointsApiExplorer();
			builder?.AddVersioning();
			builder?.Services.AddSwagger();

			builder?.Services.AddSwaggerGen();

			#region[CONFIGURACION SERVICIOS EXTERNOS HTTP/HTTPS]
			builder?.Services.AddHttpClient(ConfigurationStruct.CargaTrxMasivo, client =>
			{
				client.BaseAddress = new Uri(Environment.GetEnvironmentVariable(ConfigurationStruct.CargaTrxMasivo) ?? string.Empty);
			}).ConfigurePrimaryHttpMessageHandler(() => { return DesabilitarSSlDevQa(builder); });
			builder?.Services.AddHttpClient(ConfigurationStruct.TrackingInventario, client =>
			{
				client.BaseAddress = new Uri(Environment.GetEnvironmentVariable(ConfigurationStruct.TrackingInventario) ?? string.Empty);
			}).ConfigurePrimaryHttpMessageHandler(() => { return DesabilitarSSlDevQa(builder); });
			builder?.Services.AddHttpClient(ConfigurationStruct.Endpoint_Broker, client =>
			{
				client.BaseAddress = new Uri(Environment.GetEnvironmentVariable(ConfigurationStruct.Endpoint_Broker) ?? string.Empty);
			}).ConfigurePrimaryHttpMessageHandler(() => { return DesabilitarSSlDevQa(builder); });
			#endregion

			#region [HSTS]
			builder?.Services.AddHsts(options =>
			{
				options.ExcludedHosts.Clear();
				options.Preload = true;
				options.IncludeSubDomains = true;
				options.MaxAge = TimeSpan.FromDays(365);
			});
			#endregion


			return builder!;
		}

		/// <summary>
		/// Desabilita el SSL para Desarrollo y QA
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		private static HttpClientHandler DesabilitarSSlDevQa(WebApplicationBuilder builder)
		{
			var handler = new HttpClientHandler();

			if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
			{
				// Reemplazar ILogger<Program> con ILogger en el método DesabilitarSSlDevQa
				builder.Services.BuildServiceProvider()
					.GetRequiredService<ILogger>()
					.LogWarning(UserTypeMessages.SSL);

				handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
				{
					return sslPolicyErrors == SslPolicyErrors.None;
				};
			}

			return handler;
		}
	}
}
