// <copyright file="EsquemasJson.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Static
{
	[ExcludeFromCodeCoverage]

	public static class EsquemasJson
	{

		public const string JsonErr = @"
										{
										  ""$schema"": ""http://json-schema.org/draft-07/schema#"",
										  ""type"": ""object"",
										  ""properties"": {
											""codigo_interno"": { ""type"": ""string"" },
											""estado_interno"": { ""type"": ""string"" }
										  },
										  ""required"": [""codigo_interno"", ""estado_interno""]
										}";


		public const string JsonOK = @"

										{
										  ""$schema"": ""http://json-schema.org/draft-07/schema#"",
										  ""type"": ""object"",
										  ""properties"": {
											""codigo"": { ""type"": ""integer"" },
											""estado"": { ""type"": ""boolean"" },
											""mensaje"": { ""type"": ""string"" },
											""resultado"": {
											  ""type"": ""array"",
											  ""items"": {
												""type"": ""object"",
												""properties"": {
												  ""sticker"": { ""type"": ""string"" },
												  ""idDevolucion"": { ""type"": ""string"" },
												  ""mensaje"": { ""type"": ""string"" }
												},
												""required"": [""sticker"", ""idDevolucion"", ""mensaje""]
											  }
											}
										  },
										  ""required"": [""codigo"", ""estado"", ""mensaje"", ""resultado""]
										}";

	}
}
