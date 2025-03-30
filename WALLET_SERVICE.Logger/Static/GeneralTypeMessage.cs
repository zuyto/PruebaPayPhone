// <copyright file="GeneralTypeMessage.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Logger.Static
{
	public struct ConfigTypeMessage
	{
		public const string GENERAL01 = "\n\n {tipoError} \n {message} | DATOS: {metodo} \n | Parametros: {parametros} \n\n";
		public const string FORMATDATE = "yyyy-MM-dd";
		public const string FORMATHOUR = "HH:mm:ss";
		public const string FECHA = "Fecha";
		public const string HORA = "Hora";
		public const string NIVEL = "Nivel";
		public const string METODO = "Método";
		public const string ARCHIVO = "Archivo";
		public const string LINEA = "Línea";
		public const string LVL = "Unknown";
		public const string USUARIO = "Usuario";
		public const string ANONYMOUS = "anonymous";
		public const string LOGERGUION = "-";
		public const string META = "v1.1";
	}
}
