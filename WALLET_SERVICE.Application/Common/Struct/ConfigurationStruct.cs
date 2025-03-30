// <copyright file="ConfigurationStruct.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.Struct
{
	public struct ConfigurationStruct
	{
		public const string DbSecretDB = "SGLPROD";
		public const bool CreateSuccessfulIsSuccess = true;
		public const bool CreateSuccessfulIsError = false;
		public const bool CreateUnsuccessfulIsSuccess = false;
		public const bool CreateUnsuccessfulIsError = false;
		public const bool CreateErrorfulIsSuccess = false;
		public const bool CreateErrorIsError = true;
		public const string _contentTypeSuport = "application/json";
		public const string _contentTypeXmlSuport = "application/xml";
		public const string Accept = "Accept";

		public const string HeadersOcpApimSubscriptionKey =
			"Ocp-Apim-Subscription-Key";

		public const string sentenseDbOracle = "select ora_database_name from dual";
		public const string OraDatabaseName = "ORA_DATABASE_NAME";
		public const string SiKeyVault = "S";

		public const string TermsOfServices = "https://example.com/terms-of-service";

		public const string Contact = "https://www.payphone.app/";
		public const string License = "https://opensource.org/license/apache-2-0/";
		public const string SecretDb = "SecretDB";
		public const string WithOrigins = "WithOrigins";
		public const string CorsPolicy = "CorsPolicy";
		public const string RealIpHeader = "X-Real-IP";
		public const bool EnableEndpointRateLimiting = true;
		public const bool StackBlockedRequests = false;
		public const string Endpoint = "*";
		public const string Period = "1s";
		public const string HttpRequestExceptionMessage =
			"Ha ocurrido un error en el consumo del servicio. Status code: {0} {1} Content: {2}";

		public const string Gerencia = "Gerencia";
		public const string Celula = "Celula";
		public const string Aplicacion = "Aplicacion";
		public const string Proyecto = "Proyecto";



		#region[NOMBRES DB]
		public const string PROD_SGL = "PROD_SGL";
		public const string PROD_PROUNIPRO = "PROD_PROUNIPRO";
		public const string ABAS_ABASTECIMIENTO = "ABAS_ABASTECIMIENTO";
		public const string ABAS_HUBPRV = "ABAS_HUBPRV";
		public const string ABAS_SGL = "ABAS_SGL";
		public const string HCAV_SGL = "HCAV_SGL";
		public const string HCAV_SAPS = "HCAV_SAPS";
		public const string GSPL_ORQUESTADOR_VE = "GSPL_ORQUESTADOR_VE";
		public const string WALLET_DB_CONEXION = "WALLET_DB_CONEXION";
		#endregion


		#region[NOMBRES ENDPOINTS]
		public const string CargaTrxMasivo = "CargaTrxMasivo";
		public const string TrackingInventario = "TrackingInventario";
		public const string Endpoint_Broker = "Endpoint_Broker";
		#endregion
	}
}
