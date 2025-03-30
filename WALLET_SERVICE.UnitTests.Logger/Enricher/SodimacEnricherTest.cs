// <copyright file="SodimacEnricherTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Security.Claims;

using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;

using WALLET_SERVICE.Logger.Enricher;
using WALLET_SERVICE.Logger.Static;

namespace WALLET_SERVICE.Logger.UnitTest.Enricher
{
	public class EnricherTest
	{
		[Fact]
		public void Enrich_ShouldAddUserNameProperty_WhenIdentityHasName()
		{
			// Arrange
			var claimsIdentity = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, "TestUser")
			});
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			var enricher = new LoogerEnricher(claimsPrincipal);
			var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, new MessageTemplate("", new List<MessageTemplateToken>()), new List<LogEventProperty>());
			var propertyFactory = new PropertyFactory();

			// Act
			enricher.Enrich(logEvent, propertyFactory);

			// Assert
			Assert.Contains(logEvent.Properties, p => p.Key == ConfigTypeMessage.USUARIO && p.Value.ToString() == "\"TestUser\"");
		}

		[Fact]
		public void Enrich_ShouldAddAnonymousProperty_WhenIdentityHasNoName()
		{
			// Arrange
			var claimsIdentity = new ClaimsIdentity();
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			var enricher = new LoogerEnricher(claimsPrincipal);
			var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, new MessageTemplate("", new List<MessageTemplateToken>()), new List<LogEventProperty>());
			var propertyFactory = new PropertyFactory();

			// Act
			enricher.Enrich(logEvent, propertyFactory);

			// Assert
			Assert.Contains(logEvent.Properties, p => p.Key == ConfigTypeMessage.USUARIO && p.Value.ToString() == $"\"{ConfigTypeMessage.ANONYMOUS}\"");
		}
		[Fact]
		public void Enrich_ShouldHandleNullIdentity()
		{
			// Arrange
			var claimsPrincipal = new ClaimsPrincipal();
			var enricher = new LoogerEnricher(claimsPrincipal);
			var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, new MessageTemplate("", new List<MessageTemplateToken>()), new List<LogEventProperty>());
			var propertyFactory = new PropertyFactory();

			// Act
			enricher.Enrich(logEvent, propertyFactory);

			// Assert
			Assert.Contains(logEvent.Properties, p => p.Key == ConfigTypeMessage.USUARIO && p.Value.ToString() == $"\"{ConfigTypeMessage.ANONYMOUS}\"");
		}
	}

	internal class PropertyFactory : ILogEventPropertyFactory
	{
		/// <summary>
		/// Crear propiedad interna para Pruebas
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="destructureObjects"></param>
		/// <returns></returns>
		public LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false)
		{
			return new LogEventProperty(name, new ScalarValue(value));
		}
	}
}
