// <copyright file="Constantes.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Static
{
	[ExcludeFromCodeCoverage]

	public static class Constantes
	{

		public const string Proceso_Exitoso = "Proceso exitoso";

		public const string SEQ_TBL_OMS_CAMBIO_ESTADO_NP_AUD = "SEQ_TBL_OMS_CAMBIO_ESTADO_NP_AUD";

		public const string ConsultaObtenerSecuencia = "select (SGL.PKG_APLICACION.FNC_GET_SEQ_BY_NAME('{0}')) as Id from dual";



	}
}
