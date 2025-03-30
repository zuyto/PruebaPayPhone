// <copyright file="ConvertTo.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Newtonsoft.Json;

namespace WALLET_SERVICE.Application.Common.Helpers
{
	[ExcludeFromCodeCoverage]
	public static class ConvertTo
	{
		/// <summary>
		/// Convertir un Datatable a una lista <see cref="List{T}"/> de tipo <paramref name="T"/> especifico
		/// </summary>
		/// <typeparam name="T">Modelo</typeparam>
		/// <param name="dt">datatable</param>
		/// <returns>Lista <see cref="List{T}"/> de tipo <paramref name="T"/> especifico</returns>
		public static List<T> ConvertDataTableToList<T>(DataTable dt)
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = SetProperties.GetItem<T>(row);
				data.Add(item);
			}
			return data;
		}

		/// <summary>
		/// Convertir un Datatable al tipo objeto <see cref="T"/> <paramref name="T"/> especifico
		/// </summary>
		/// <remarks>Tener encuenta que solo funciona cuando el DataTable es de una Fila.</remarks>
		/// <typeparam name="T">Modelo</typeparam>
		/// <param name="dt">datatable</param>
		/// <returns>Objecto <see cref="T"/> de tipo <paramref name="T"/> especifico</returns>
		public static T ConvertDataTableToObject<T>(DataTable dt)
		{
			T? data = (T?)Activator.CreateInstance(typeof(T));
			foreach (DataRow row in dt.Rows)
			{
				data = SetProperties.GetItem<T>(row);
			}
			return data!;
		}

		/// <summary>
		/// Convierte una Lista en DataTable
		/// </summary>
		/// <typeparam name="T">Objeto al que se desea convertir, nombre de las columnas en su respectivo orden</typeparam>
		/// <param name="items">Lista a convertir a datatable</param>
		/// <returns></returns>
		public static DataTable ConvertListToDataTable<T>(List<T> items)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);

			PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo prop in Props)
			{
				dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
			}
			foreach (T item in items)
			{
				var values = new object[Props.Length];
				for (int i = 0; i < Props.Length; i++)
				{
					values[i] = Props[i].GetValue(item) ?? DBNull.Value;
				}
				dataTable.Rows.Add(values);
			}
			return dataTable;
		}

		/// <summary>
		/// Convierte un Object en DataTable
		/// </summary>
		/// <typeparam name="T">Objeto al que se desea convertir, nombre de las columnas en su respectivo orden</typeparam>
		/// <param name="items">Lista a convertir a datatable</param>
		/// <returns></returns>
		public static DataTable ConvertObjectToDataTable<T>(T items)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);

			PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo prop in Props)
			{
				dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
			}

			var values = new object[Props.Length];
			for (int i = 0; i < Props.Length; i++)
			{
				values[i] = Props[i].GetValue(items) ?? DBNull.Value;
			}
			dataTable.Rows.Add(values);

			return dataTable;
		}

		/// <summary>
		/// Convierte un DataTable en un Diccionario de datos
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static List<Dictionary<string, object>>? ConvertDataTableToDictionary(DataTable dt)
		{
			if (dt != null)
			{
				return dt.AsEnumerable()?
						 .Select(r => r.Table.Columns.Cast<DataColumn>()
						 .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal]))
						 .ToDictionary(z => z.Key, z => z.Value)).ToList()
						 ?? new List<Dictionary<string, object>> { new Dictionary<string, object> { { "MENSAJE", "El DataTable recibido no contiene datos" } } };
			}
			else
			{
				return new List<Dictionary<string, object>>
				{
					new Dictionary<string, object>
					{
						{ "MENSAJE", "El DataTable recibido no contiene datos" }
					}
				};
			}

		}


		/// <summary>
		/// Convierte una lista tipada a cadena de texto
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="lista"></param>
		/// <param name="separadorElementos"> Este es el separador que se utiliza para separar los diferentes elementos de la lista.</param>
		/// <param name="separadorPropiedades">Este es el separador que se utiliza para separar las propiedades de un objeto dentro de la lista.</param>
		/// <param name="inicio">Este parámetro controla si se agrega el separadorPropiedades al inicio de cada propiedad</param>
		/// <returns></returns>
		public static string ConvertListToTextString<T>(IEnumerable<T> lista, string separadorElementos = "|", string separadorPropiedades = ",", bool inicio = false) where T : class
		{
			if (lista == null || !lista.Any()) return string.Empty;

			var sb = new StringBuilder();
			foreach (var objeto in lista.Where(o => o != null))
			{
				sb.Append(ConstruirCadenaObjeto(objeto, separadorPropiedades, inicio))
				  .Append(separadorElementos);
			}

			// Eliminar el último separador
			return sb.ToString(0, sb.Length - separadorElementos.Length);
		}

		/// <summary>
		/// Convierte un objeto a cadena de texto
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objeto"></param>
		/// <param name="separadorPropiedades"></param>
		/// <param name="inicio"></param>
		/// <returns></returns>
		private static string ConstruirCadenaObjeto<T>(T objeto, string separadorPropiedades, bool inicio) where T : class
		{
			var propiedades = objeto.GetType().GetProperties();
			var valores = propiedades
				.Select(p => p.GetValue(objeto)?.ToString() ?? "0");

			return inicio
				? separadorPropiedades + string.Join(separadorPropiedades, valores)
				: string.Join(separadorPropiedades, valores);
		}

		/// <summary>
		/// Conviuerte una lista primitova a cadena de texto
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="lista"></param>
		/// <param name="separadorElementos"></param>
		/// <returns></returns>
		public static string ConvertListPrimitivaToTextString<T>(IEnumerable<T> lista, string separadorElementos = ",")
		{
			return string.Join(separadorElementos, lista);
		}

		/// <summary>
		/// Metodo para convertir JsonString en string XML  
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static XmlDocument ConvertJsonStringToXML(string json)
		{
			XmlDocument doc = new XmlDocument();

			using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max))
			{
				XElement xml = XElement.Load(reader);
				doc.LoadXml(xml.ToString());
			}

			return doc;
		}

		/// <summary>
		/// Metodo para convertir clase en string XML
		/// </summary>
		/// <param name="clase">clase que se va a convertir</param>
		/// <param name="rootName">nombre del root del XML</param>
		/// <returns>un string en formato XML</returns>
		public static string? ConvertObjectToXml(object clase, string rootName)
		{
			string par = JsonConvert.SerializeObject(clase);
			string json = @par;
			XNode? node = JsonConvert.DeserializeXNode(json, rootName);
			string nodestr = GenericConstants.ComplementoXML + node;
			return nodestr;
		}

		/// <summary>
		/// Metodo encargado de convertir un tipo de dato a base 64 string
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dato"></param>
		/// <returns></returns>
		public static string? ConvertToBase64<T>(T dato)
		{

			byte[] bytes = Encoding.UTF8.GetBytes(dato?.ToString()!);

			return Convert.ToBase64String(bytes);

		}

		/// <summary>
		/// realiza el substring de un string de forma segura
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string? SafeSubstringString(this string? str, int length)
		{

			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}

			return str.Substring(0, Math.Min(str.Length, length));

		}


		/// <summary>
		/// realiza el substring de un int de forma segura
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static int? SafeSubstringInt(this int? number, int length)
		{
			if (number == null)
			{
				return null;
			}

			string numberString = number.ToString()!;
			string substring = numberString.Substring(0, Math.Min(numberString.Length, length));

			return int.TryParse(substring, out int result) ? result : (int?)null;
		}

		/// <summary>
		/// realiza el substring de un decimal de forma segura
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static decimal? SafeSubstringDecimal(this decimal? number, int length)
		{
			if (number == null)
			{
				return null;
			}

			string numberString = number.ToString()!;
			string substring = numberString.Substring(0, Math.Min(numberString.Length, length));

			return decimal.TryParse(substring, out decimal result) ? result : (decimal?)null;
		}

		/// <summary>
		/// realiza el substring de un float de forma segura
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static float? SafeSubstringFloat(this float? number, int length)
		{
			if (number == null)
			{
				return null;
			}

			string numberString = number.ToString()!;
			string substring = numberString.Substring(0, Math.Min(numberString.Length, length));

			return float.TryParse(substring, out float result) ? result : (float?)null;
		}

		/// <summary>
		/// realiza el substring de un double de forma segura
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static double? SafeSubstringDouble(this double? number, int length)
		{
			if (number == null)
			{
				return null;
			}

			string numberString = number.ToString()!;
			string substring = numberString.Substring(0, Math.Min(numberString.Length, length));

			return double.TryParse(substring, out double result) ? result : (double?)null;
		}

		/// <summary>
		/// realiza el substring de un double de forma segura
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static long? SafeSubstringLong(this long? number, int length)
		{
			if (number == null)
			{
				return null;
			}

			string numberString = number.ToString()!;
			string substring = numberString.Substring(0, Math.Min(numberString.Length, length));

			return long.TryParse(substring, out long result) ? result : (long?)null;
		}

		/// <summary>
		/// Convierte un striing en DateTime 
		/// </summary>
		/// <remarks>
		/// FORMATOS FECHA ACEPTADOS: "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "yyyy-MM-dd", "dd-MM-yyyy"
		/// 
		/// FORMATOS HORA ACEPTADOS:  " HH:mm", " H:mm", " HH:mm:ss", " H:mm:ss", " "
		/// </remarks>
		/// <param name="fecha"></param>
		/// <returns></returns>
		public static DateTime? ConvertStringToDatetime(string fecha)
		{

			var formatos = GenerarFormatosFecha();

			DateTime.TryParseExact(fecha, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaConvertida);

			return fechaConvertida;

		}

		/// <summary>
		/// Metodo encargado de recorrer las diferentes conbinaciones posibles con los formatos de fecha Aceptados
		/// </summary>
		/// <returns></returns>
		private static string[] GenerarFormatosFecha()
		{
			List<string> formatos = new List<string>();
			string[] patronesFecha = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "yyyy-MM-dd", "dd-MM-yyyy" };
			string[] patronesHora = { " HH:mm", " H:mm", " HH:mm:ss", " H:mm:ss", " hh:mm tt", " h:mm tt", " hh:mm:ss tt", " h:mm:ss tt", " " };


			foreach (string patronFecha in patronesFecha)
			{
				foreach (string patronHora in patronesHora)
				{
					formatos.Add(patronFecha + patronHora);
				}

				formatos.Add(patronFecha);
			}

			return [.. formatos];
		}

	}
}
