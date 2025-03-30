// <copyright file="GenericHelpers.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using Oracle.ManagedDataAccess.Client;

using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
using WALLET_SERVICE.Application.Common.Static;

namespace WALLET_SERVICE.Application.Common.Helpers
{
	[ExcludeFromCodeCoverage]
	public static class GenericHelpers
	{
		/// <summary>
		/// Método encargado de Armar la respuesta generica
		/// </summary>
		/// <param name="esExitoso">True si la respuesta es exitosa o False si la respuesta es fallida</param>
		/// <param name="response">Respuesta que se desea mostrar como resultado <see cref="object"/></param>
		/// <param name="mensaje">Mensaje que se desea mostrar en la respuesta</param>
		/// <returns>Objeto de tipo <see cref="DtoGenericResponse"/> como respuesta generica</returns>
		public static DtoGenericResponse<T> BuildResponse<T>(bool esExitoso, T? response, string mensaje, bool isOkEsquema = false)
		{
			// Intentar convertir el objeto anónimo al tipo T
			T? result = response is T castedResponse ? castedResponse : default;

			return new DtoGenericResponse<T>
			{
				EsExitoso = esExitoso,
				Resultado = result,
				Mensaje = mensaje,
				IsOkEsquema = isOkEsquema
			};
		}

		/// <summary>
		/// Método encargado de Armar la respuesta generica
		/// </summary>
		/// <param name="esExitoso">True si la respuesta es exitosa o False si la respuesta es fallida</param>
		/// <param name="response">Respuesta que se desea mostrar como resultado <see cref="object"/></param>
		/// <param name="mensaje">Mensaje que se desea mostrar en la respuesta</param>
		/// <returns>Objeto de tipo <see cref="DtoGenericResponse"/> como respuesta generica</returns>
		public static DtoGenericResponseHttp BuildResponseHttp(bool esExitoso, object? response, string mensaje, string? responseHttp)
		{
			return new DtoGenericResponseHttp
			{
				EsExitoso = esExitoso,
				Mensaje = mensaje,
				Resultado = response,
				ResultadoHttp = responseHttp
			};
		}

		/// <summary>
		/// Consulta la excepcion contenida mas interna y saca de esta su información.
		/// </summary>
		/// <param name="exception">Excepción a ser analizada.</param>
		/// <returns>retorna un mensaje en string</returns>
		public static DtoErrorResponse HandleExceptionMessage(this Exception exception, bool includeSensitiveInformation = false)
		{
			Exception? l = null;
			Exception? e = exception;

			while (e != null)
			{
				l = e;
				e = e.InnerException;
			}

			var response = new DtoErrorResponse
			{
				TipoExcepcion = l?.GetType().ToString() ?? string.Empty,
				Mensaje = l?.Message ?? string.Empty,
				Objeto = l?.TargetSite?.Module.Name ?? string.Empty,
				Metodo = l?.TargetSite?.Name ?? string.Empty
			};

			if (includeSensitiveInformation)
			{
				response.Stacktrace = l?.StackTrace ?? string.Empty;

				if (l?.InnerException != null)
				{
					response.DetalleInnerException = l.InnerException.ToString();
				}
				else
				{
					response.DetalleInnerException = string.Empty;
				}
			}

			return response;
		}

		/// <summary>
		/// Metodo encargado de optener la respuesta por parámetro de la clase repositorio
		/// </summary>
		/// <param name="parametros"></param>
		/// <returns></returns>
		public static object? GetResulParam(OracleParameter[] parametros)
		{
			object? res = null;
			for (int i = 0; i < parametros.Length; i++)
			{
				if (parametros[i].ParameterName == OracleMappingConstants.P_RESULTADO || parametros[i].ParameterName == OracleMappingConstants.P_SALIDA)
				{
					res = string.IsNullOrEmpty(parametros[i].Value.ToString()) ? null : parametros[i].Value;
					return res;
				}
			}

			return res;
		}

		/// <summary>
		/// Validar si el esquema Json coincide
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static bool ValidarEsquemaErr(string json)
		{
			try
			{
				JSchema schema = JSchema.Parse(EsquemasJson.JsonErr);

				JObject jsonJObject = JObject.Parse(json);

				if (jsonJObject.IsValid(schema))
				{
					return true;
				}
				else { return false; }
			}
			catch (Exception)
			{
				return false;
			}



		}

		/// <summary>
		/// Validar si el esquema Json coincide
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static bool ValidarEsquemaOk(string json)
		{

			try
			{
				JSchema schema = JSchema.Parse(EsquemasJson.JsonOK);

				JObject jsonJObject = JObject.Parse(json);

				if (jsonJObject.IsValid(schema))
				{
					return true;
				}
				else { return false; }
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Intenta Deserializar un string en <see cref="T"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static dynamic? TryDeserialize<T>(string json) where T : class
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch
			{
				return json;
			}
		}

		/// <summary>
		/// Deserializar un objeto a un objeto tipado
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="deserializeOnject"></param>
		/// <returns></returns>
		public static T DeserializeObjects<T>(object? deserializeOnject) where T : new()
		{

			try
			{
				string obj = JsonConvert.SerializeObject(deserializeOnject);

				return JsonConvert.DeserializeObject<T>(obj) ?? new T();
			}
			catch
			{

				return new T();
			}


		}

		/// <summary>
		/// verificar si un dato es nulo 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="valor"></param>
		/// <param name="asignar"></param>
		public static void AsignarSiNoEsNulo<T>(T? valor, Action<T> asignar) where T : struct
		{
			if (valor.HasValue && !valor.Equals(default(T)))
			{
				asignar(valor.Value);
			}
		}

		/// <summary>
		/// verifica si un dato es nulo o vacio
		/// </summary>
		/// <param name="valor"></param>
		/// <param name="asignar"></param>
		public static void AsignarSiNoEsNuloOVacio(string valor, Action<string> asignar)
		{
			if (!string.IsNullOrWhiteSpace(valor))
			{
				asignar(valor);
			}
		}

		/// <summary>
		/// Separa una cadena de texto por un separador 
		/// </summary>
		/// <param name="cadenaIni"></param>
		/// <param name="separador"></param>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static string FncSplit(this string cadenaIni, char separador, int tag)
		{
			var sb = new StringBuilder();
			var count = 1;

			for (int i = 0; i < cadenaIni.Length; i++)
			{
				var temp = cadenaIni[i];
				if (temp != separador)
				{
					sb.Append(temp);
				}
				else if (count == tag)
				{
					break;
				}
				else
				{
					count++;
					sb.Clear();
				}
			}

			return sb.ToString().Trim();
		}

	}
}
