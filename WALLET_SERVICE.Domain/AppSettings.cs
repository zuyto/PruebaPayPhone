// <copyright file="AppSettings.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Newtonsoft.Json;

namespace WALLET_SERVICE.Domain
{
	public class AppSettings
	{
		public ConnectionStrings ConnectionStrings { get; set; } = null!;
		public Logging Logging { get; set; } = null!;
		public string AllowedHosts { get; set; } = null!;
		public bool EnableRequestResponseLogging { get; set; }
		public List<string> WithOrigins { get; set; } = null!;
		public AppSettingsConfig? AppSettingsConfig { get; set; }


	}

	public class ConnectionStrings
	{
		public string SecretDB { get; set; } = null!;
		public string? PROD_SGL { get; set; }
		public string? HCAV_SGL { get; set; }

	}

	public class Logging
	{
		public LogLevel? LogLevel { get; set; }
	}

	public class LogLevel
	{
		public string? Default { get; set; }

		[JsonProperty("Microsoft.AspNetCore")]
		public string? MicrosoftAspNetCore { get; set; }
	}


}
