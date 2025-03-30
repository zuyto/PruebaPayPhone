// <copyright file="IDynamicRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.Interfaces.Repository
{
	public interface IDynamicRepository
	{
		/// <summary>
		///     Ejecutar sentencia en database
		/// </summary>
		/// <param name="sentence"></param>
		/// <param name="secreto"></param>
		/// <returns></returns>
		public Task<List<object>> ExecuteSentenciaOnDatabase(string sentence,
			string secreto);

		/// <summary>
		///     Prueba de conexión
		/// </summary>
		/// <param name="secreto"></param>
		/// <returns></returns>
		public Task<bool> TestConnectionDynamic(string secreto);
	}
}
