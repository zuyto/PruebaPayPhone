// <copyright file="ApiResponseBase.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.CustomEntities;

namespace WALLET_SERVICE.Api.Response
{
	/// <summary>
	///     ApiResponseBase
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResponseBase<T>
	{
		/// <summary>
		///     OkMessage
		/// </summary>
		public string? OkMessage { get; set; }

		/// <summary>
		///     IsSuccessful
		/// </summary>
		public bool IsSuccessful { get; set; }

		/// <summary>
		///     ErrorMessage
		/// </summary>
		public string? ErrorMessage { get; set; }

		/// <summary>
		///     IsError
		/// </summary>
		public bool IsError { get; set; }


		/// <summary>
		///     Messages
		/// </summary>
		public IEnumerable<string>? Messages { get; set; }

		/// <summary>
		///     Meta
		/// </summary>
		public Metadata? Meta { get; set; }

		/// <summary>
		/// </summary>
		public T? Result { get; set; }

	}
}
