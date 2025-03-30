// <copyright file="ApiResponse.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Api.Response
{
	/// <summary>
	///     ApiResponse
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResponse<T> : ApiResponseBase<T>
	{
		/// <summary>
		///     constructor
		/// </summary>
		public ApiResponse()
		{
		}

		/// <summary>
		///     ApiResponse
		/// </summary>
		/// <param name="isSuccessful"></param>
		/// <param name="isError"></param>
		/// <param name="errorMessage"></param>
		/// <param name="messages"></param>
		/// <param name="result"></param>
		/// <param name="okMessage"></param>
		public ApiResponse(bool isSuccessful, bool isError, string? errorMessage,
			IEnumerable<string>? messages, T? result, string? okMessage = "")
		{
			IsSuccessful = isSuccessful;
			IsError = isError;
			ErrorMessage = errorMessage;
			Messages = messages;
			Result = result;
			OkMessage = okMessage;
			Meta = null;

		}


		/// <summary>
		///     CreateSuccessful
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public static ApiResponse<T> CreateSuccessful(T result)
		{
			return new ApiResponse<T>(ConfigurationStruct.CreateSuccessfulIsSuccess,
				ConfigurationStruct.CreateSuccessfulIsError, null, null, result);
		}

		/// <summary>
		/// CreateSuccessful 
		/// </summary>
		/// <param name="result"></param>
		/// <param name="okMsg"></param>
		/// <returns></returns>
		public static ApiResponse<T> CreateSuccessful(T result, string okMsg) => new(ConfigurationStruct.CreateSuccessfulIsSuccess, ConfigurationStruct.CreateSuccessfulIsError, null, null, result, okMsg);



		/// <summary>
		///     CreateUnsuccessful
		/// </summary>
		/// <param name="messages"></param>
		/// <returns></returns>
		public static ApiResponse<T> CreateUnsuccessful(
			IEnumerable<string> messages)
		{
			return new ApiResponse<T>(
				ConfigurationStruct.CreateUnsuccessfulIsSuccess,
				ConfigurationStruct.CreateUnsuccessfulIsError, null, messages,
				default);
		}

		/// <summary>
		/// CreateUnsuccessful
		/// </summary>
		/// <param name="messages"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public static ApiResponse<T> CreateUnsuccessful(T messages, string? errorMessage) => new(isSuccessful: false, isError: true, errorMessage, null, messages);

		/// <summary>
		///     CreateError
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public static ApiResponse<T> CreateError(string errorMessage)
		{
			return new ApiResponse<T>(ConfigurationStruct.CreateErrorfulIsSuccess,
				ConfigurationStruct.CreateErrorIsError, errorMessage, null,
				default);
		}

		/// <summary>
		/// CreateError con resultado
		/// </summary>
		/// <param name="message"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static ApiResponse<T> CreateError(string message, T result) => new(isSuccessful: false, isError: true, message, null, result);


	}
}
