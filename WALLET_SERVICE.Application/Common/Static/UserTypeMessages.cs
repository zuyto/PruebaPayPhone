// <copyright file="UserTypeMessages.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.Static
{
	/// <summary>
	///     Mensajes de control de errores de la aplicación
	/// </summary>
	public static class UserTypeMessages
	{
		public const string ERRGEN01 = "ERRGEN01 - Ocurrió un error general en la aplicación";
		public const string ERRGEN02 = "ERRGEN02 - ERROR! de excepcion en el metodo {0} | Parametros: {1} | Mensaje: {2}";
		public const string ERRGEN03 = "ERRGEN03 - ERROR! Error en el modelo de entrada";
		public const string ERRGEN04 = "ERRGEN04 - Critical! En el metodo {0} | Parametros: {1} | Mensaje: {2}";
		public const string ERRGEN05 = "ERRGEN05 - Warning! En el metodo {0} | Parametros: {1} | Mensaje: {2}";
		public const string ERRGEN06 = "ERRGEN06 - EL origen no coincide";


		public const string INFGENO01 = "INFGENO01 - Procesamiento del metodo {0}";
		public const string INFGENO02 = "INFGENO02 - Procesamiento del metodo {0} | Parametros: {1} | Estado: {2}";
		public const string INFGENO03 = "INFGENO03 - Procesamiento del metodo {0} | Parametros: {1} | Estado del metodo: {2}";

		public const string? SSL = "⚠️ La validación del certificado SSL está deshabilitada en entornos de desarrollo y pruebas.";

		public const string OKGEN01 = "PROCESO REALIZADO CON EXITO";
		public const string OKERRGEN01 = "PROCESO REALIZADO CON EXITO, PERO PRESENTO ALGUNAS NOVEDADES";
		public const string ERROR_NO_DATA_REDES = "NO SE ENCONTRARON REDES DISPONIBLES";
		public const string ERROR_NO_CONTROLADO = "SE HA PRESENTADO UN ERROR NO CONTROLADO";
		public const string ERROR_REQUEST = "LOS DATOS DEL REQUEST ENVIADO ES NULO O EL FORMATO ENVIADO ES INCORRECTO";
		public const string NULL_BODY_REQUEST = "EL CUERPO DEL REQUEST NO PUEDE SER NULO.";


		public const string ERRGEN07 = "LA CONSULTA NO RETORNO DATOS";
		public const string ERRGEN08 = "TIMEOUT AL INTENTAR REALIZAR LA SOLICITUD HTTP";
		public const string ERRCATHCONTROLLER = "**** CONTROLLER RESPONSE CATCH {0} ****";
		public const string CONTROLLER_RESPONSE = "**** CONTROLLER RESPONSE {0} ****";


		public const string ERRUSRTP02 = "EL ID TRANSPORTADORA NO COINSIDE PARA CONSUMIR EL SERVICIO EXTERNO";


		public const string NO_DATOS = "NO SE RETORNARON DATOS";
		public const string ERROR_API_EXTERNA = "ERROR EN EL API EXTERNA";
		public const string ERROR_OBTENER_DATOS = "ERROR PRESENTADO AL OBTENER LOS DATOS";

		public const string API_NO_RETORNA_DATOS = "NO SE RETORNARON DATOS PARA LA GENERACIÓN DE LA GUIA";
		public const string ERROR_NO_GUIA = "ERROR EN EL PROCESO DE GENERACIÓN DE GUÍAS";
		public const string ERROR_CREA_GUIA = "LA RESPUESTA DE LA TRANSPORTADORA NO CONTIENE UNA GUIA";

		public const string ERROR_REINTENTOS = "DESCARTADO POR MAXIMO DE REINTENTOS";

		public const string? ENVIADO = "VIAJE ENVIADO CON EXITO";






		public const string? ERROR_CERO_MONTO = "EL MONTO ES CERO(0) Y NO ES POSIBLE HACER SU TRANSACCIÓN";

		public const string ERROR_WALLET_ORIGEN_INVALIDO = "BILLETERA DE ORIGEN NO ENCONTRADA";
		public const string ERROR_WALLET_DESTINO_INVALIDO = "BILLETERA DE DESTINO NO ENCONTRADA";

		public const string SALDO_INSUFICIENTE = "SALDO INSUFICIENTE EN LA BILLETERA DE ORIGEN";
	}
}
