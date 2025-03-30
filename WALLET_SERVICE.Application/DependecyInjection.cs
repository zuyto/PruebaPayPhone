// <copyright file="DependecyInjection.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

using WALLET_SERVICE.Application.Common.Helpers;
using WALLET_SERVICE.Application.Common.Interfaces.Services;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Http;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Ping;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Profiles;
using WALLET_SERVICE.Application.Services;
using WALLET_SERVICE.Application.Services.Http;
using WALLET_SERVICE.Application.Services.Ping;
using WALLET_SERVICE.Application.Services.Serilog;

namespace WALLET_SERVICE.Application
{
	[ExcludeFromCodeCoverage]
	public static class DependecyInjection
	{
		public static IServiceCollection AddApplication(
			this IServiceCollection services)
		{
			_ = services.AddFluentValidationAutoValidation()
				.AddFluentValidationClientsideAdapters();

			_ = services.AddAutoMapper(
				Assembly.GetAssembly(typeof(MappingProfile)));

			services.AddTransient<IPingServices, PingServices>();
			services.AddTransient<ISerilogImplements, SerilogImplements>();
			services.AddTransient<IGenericServiceAgent, GenericServiceAgent>();
			services.AddTransient<HttpServiceManager>();
			//services.AddTransient<IBifurcacionDatosPromiseEngine, BifurcacionDatosPromiseEngine>();
			services.AddTransient<IWalletService, WalletService>();

			return services;
		}
	}
}
