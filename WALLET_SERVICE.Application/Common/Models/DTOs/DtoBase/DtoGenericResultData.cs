// <copyright file="DtoGenericResultData.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase
{
	/// <summary>
	/// Clase encargada de mapear errores en los metodos 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[ExcludeFromCodeCoverage]
	public class DtoGenericResultData<T>
	{
		public T? Dto { get; set; }
		public string? ErrorMessage { get; set; }
		public bool IsError { get; set; }

		[ExcludeFromCodeCoverage]

		// Método estático para construir instancias de DtoGenericResultData<T>
		public static DtoGenericResultData<T> BuildResultData(bool isError, T dto, string errorMessage)
		{
			return new DtoGenericResultData<T>
			{
				Dto = dto,
				ErrorMessage = errorMessage,
				IsError = isError
			};
		}

	}
}
