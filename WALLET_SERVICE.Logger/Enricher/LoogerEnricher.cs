// <copyright file="LoogerEnricher.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Security.Claims;

using Serilog.Core;
using Serilog.Events;

using WALLET_SERVICE.Logger.Static;

namespace WALLET_SERVICE.Logger.Enricher
{
	internal class LoogerEnricher : ILogEventEnricher
	{
		private readonly ClaimsPrincipal _currentUser;

		public LoogerEnricher() : this(new ClaimsPrincipal())
		{
		}

		public LoogerEnricher(ClaimsPrincipal currentUser)
		{
			_currentUser = currentUser;
		}

		public void Enrich(LogEvent logEvent,
			ILogEventPropertyFactory propertyFactory)
		{
			var identity = _currentUser.Identity;
			var property = propertyFactory.CreateProperty(ConfigTypeMessage.USUARIO,
				identity != null && identity.Name != null
					? identity.Name
					: ConfigTypeMessage.ANONYMOUS);
			logEvent.AddPropertyIfAbsent(property);
		}
	}
}
