// <copyright file="IBifurcacionDatosPromiseEngine.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Models.DTOs;

namespace WALLET_SERVICE.Application.Common.Interfaces.Bifurcacion
{
	public interface IBifurcacionDatosPromiseEngine
	{
		/// <summary>
		/// Metodo encargado de crear un grupo de grupos
		/// </summary>
		/// <param name="poolDatosProductCobert"></param>
		/// <returns></returns>
		List<DtoGrupoProcesarCont> BifurcacionArmarGrupos(List<DtoProductoConCobertura> poolDatosProductCobert);
		/// <summary>
		/// Consulta en HCINV los productos y coberturaras existentes
		/// </summary>
		/// <param name="rqs">datos que envia en la peticion de entrada</param>
		/// <returns></returns>
		Task<List<DtoProductoConCobertura>> BifurcacionDatosProdCobert(DtoTransferJsonRequest rqs);
		/// <summary>
		/// Armar una lista de datos de tipo DtoProductoConCobertura para el response
		/// </summary>
		/// <param name="gruposProcesar"></param>
		/// <param name="poolDatosProductCobert"></param>
		/// <returns></returns>
		List<DtoProductoConCobertura> BufurcacionDatosArmarResponse(List<DtoGrupoProcesarCont> gruposProcesar, List<DtoProductoConCobertura> poolDatosProductCobert);
		/// <summary>
		/// Se encarga de armar el Json response del Servicio
		/// </summary>
		/// <param name="datosArmarResponse"></param>
		/// <returns></returns>
		List<DtoGrupoEntregaCont> BifurcacionJsonResponse(List<DtoProductoConCobertura> datosArmarResponse);
	}
}
