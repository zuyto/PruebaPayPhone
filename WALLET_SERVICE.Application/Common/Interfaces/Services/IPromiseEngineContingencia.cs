// <copyright file="IPromiseEngineContingencia.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Models.DTOs;
using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;

namespace WALLET_SERVICE.Application.Common.Interfaces.Services
{
	public interface IPromiseEngineContingencia
	{
		Task<DtoGenericResponse<DtoTransferJsonResponse>> ProcesarContingencia(DtoTransferJsonRequest request);
	}
}
