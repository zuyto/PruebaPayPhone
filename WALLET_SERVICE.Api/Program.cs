// <copyright file="Program.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using AspNetCoreRateLimit;

using WALLET_SERVICE.Api;
using WALLET_SERVICE.Api.DependecyInjectionGlobal;
using WALLET_SERVICE.Api.Middleware;
using WALLET_SERVICE.Application.Common.Struct;


var builder = WebApplication.CreateBuilder(args);

var SwaggerMajorVersion = Convert.ToInt32(Environment.GetEnvironmentVariable("SwaggerMajorVersion"));
var SwaggerMinorVersion = Convert.ToInt32(Environment.GetEnvironmentVariable("SwaggerMinorVersion"));
var AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;


builder.AddPresentation();

var app = builder.Build();

app.UseSwagger(options =>
		options.RouteTemplate = "swagger/{documentName}/swagger.json")
	.UseSwaggerUI(c =>
	{
		c.DocumentTitle = Resources.DocumentTitle;
		c.HeadContent = Resources.HeadContent;
		c.SwaggerEndpoint($"/swagger/v{SwaggerMajorVersion}/swagger.json", $"{AssemblyName} API v{SwaggerMajorVersion}.{SwaggerMinorVersion}");
		c.InjectStylesheet("/assets/swagger-ui.css");
		c.DisplayRequestDuration();
		c.DefaultModelsExpandDepth(-1);
		c.EnableDeepLinking();
		c.ConfigObject.AdditionalItems.Add("showCommonExtensions", true);
	});


app.Use(async (context, next) =>
{
	context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
	await next.Invoke();
});

app.UseHsts();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(ConfigurationStruct.CorsPolicy);

app.UseIpRateLimiting();

app.UseRequestResponseLogging();

app.UseMiddleware<NullBodyRequestMiddleware>();

app.MapHealthChecks("/api/health");

await app.RunAsync();

/// <summary>
/// Clase principal del programa.
/// </summary>
[ExcludeFromCodeCoverage]
public static partial class Program
{
	// Constructor estático para evitar la creación de instancias.
	static Program() { }
}
