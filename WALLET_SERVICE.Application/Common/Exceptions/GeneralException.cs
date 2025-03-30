// <copyright file="GeneralException.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Newtonsoft.Json;

namespace WALLET_SERVICE.Application.Common.Exceptions
{
	/// <summary>
	///     General Exception
	/// </summary>
	public class GeneralException : Exception
	{
		private const string DefaultMessage = "La entidad no existe.";

		public GeneralException() : base(DefaultMessage) { }

		public GeneralException(string message) : base(message) { }

		public GeneralException(string message, Exception innerException) : base(message, innerException) { }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		public static GeneralException FromJson(string json)
		{
			var data = JsonConvert.DeserializeAnonymousType(json, new { Message = "" });
			return new GeneralException(data!.Message);
		}

	}
}
