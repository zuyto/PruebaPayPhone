// <copyright file="SetProperties.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace WALLET_SERVICE.Application.Common.Helpers
{
	[ExcludeFromCodeCoverage]
	public static class SetProperties
	{
		/// <summary>
		/// Recorre las propiedaded del modelo y las setea
		/// </summary>
		/// <typeparam name="T">Modelo</typeparam>
		/// <param name="dr">datarow</param>
		/// <returns></returns>
		public static T GetItem<T>(DataRow dr)
		{
			Type temp = typeof(T);
			T obj = Activator.CreateInstance<T>();

			foreach (DataColumn column in dr.Table.Columns)
			{

				var prop = (from propiedades in temp.GetProperties()
							where string.Equals(propiedades.Name, column.ColumnName, StringComparison.OrdinalIgnoreCase)
							select new
							{
								tipoProp = propiedades.PropertyType,
								pro = propiedades
							}).ToList();


				Type tipoDr = dr[column.ColumnName].GetType();


				if (prop.Count != 0)
					if (prop[0].tipoProp != tipoDr)
					{
						switch (prop[0].tipoProp.ToString().Replace("System.", ""))
						{
							case "String":
								string? valorstr = dr[column.ColumnName].ToString();
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorstr), null as object[]);
								break;
							case "Nullable`1[Int64]":
								long? valor64 = getRowLong(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valor64), null as object[]);
								break;
							case "Int64":
								long? valor64N = getRowLong(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valor64N), null as object[]);
								break;
							case "Nullable`1[Int32]":
								int? valor32 = getRowInt(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valor32), null as object[]);
								break;
							case "Int32":
								int? valor32N = getRowInt(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valor32N), null as object[]);
								break;
							case "Nullable`1[Decimal]":
								decimal? valorDecimal = getRowDecimal(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorDecimal), null as object[]);
								break;
							case "Decimal":
								decimal? valorDecimalN = getRowDecimal(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorDecimalN), null as object[]);
								break;
							case "Boolean":
								bool? valorBoolean = Convert.ToBoolean(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorBoolean), null as object[]);
								break;
							case "Nullable":
								string valorNull = string.Empty;
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorNull), null as object[]);
								break;
							case "Nullable`1[DateTime]":
								DateTime? valorDateTime = getRowDateTime(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorDateTime), null as object[]);
								break;
							case "DateTime":
								DateTime? valor = getRowDateTime(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valor), null as object[]);
								break;
							case "Float":
								float? valorFloat = getRowFloat(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorFloat), null as object[]);
								break;
							case "Nullable`1[Float]":
								float? valorFloatN = getRowFloat(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorFloatN), null as object[]);
								break;
							case "Double":
								double? valorDouble = getRowDouble(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorDouble), null as object[]);
								break;
							case "Nullable`1[Double]":
								double? valorDoubleN = getRowDouble(dr[column.ColumnName]);
								prop[0].pro.SetValue(obj, RuntimeHelpers.GetObjectValue(valorDoubleN), null as object[]);
								break;

						}
					}
					else
					{
						prop[0].pro.SetValue(obj, dr[column.ColumnName], null);
					}
			}
			return obj;
		}


		/// <summary>
		/// Validar y setear el valor de un row tipo int nullable
		/// </summary>
		/// <param name="objeto">Objeto row</param>
		/// <returns></returns>
		private static int? getRowInt(object objeto)
		{
			if (objeto != null && !string.IsNullOrEmpty(objeto.ToString()))
			{
				int resultado;
				return int.TryParse(objeto.ToString(), NumberStyles.None, CultureInfo.CurrentCulture, out resultado) ? resultado : null;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Validar y setear el valor de un row tipo DateTime nullable
		/// </summary>
		/// <param name="objeto">Objeto row</param>
		/// <returns></returns>
		private static DateTime? getRowDateTime(object objeto)
		{
			if (objeto != null && !string.IsNullOrEmpty(objeto.ToString()))
			{
				DateTime resultado;
				return DateTime.TryParse(objeto.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out resultado) ? resultado : null;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Validar y setear el valor de un row tipo Decimal nullable
		/// </summary>
		/// <param name="objeto">Objeto row</param>
		/// <returns></returns>
		private static decimal? getRowDecimal(object objeto)
		{
			if (objeto != null && !string.IsNullOrEmpty(objeto.ToString()))
			{
				decimal resultado;
				return decimal.TryParse(objeto.ToString(), NumberStyles.None, CultureInfo.CurrentCulture, out resultado) ? resultado : null;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Validar y setear el valor de un row tipo int nullable
		/// </summary>
		/// <param name="objeto">Objeto row</param>
		/// <returns></returns>
		private static long? getRowLong(object objeto)
		{
			if (objeto != null && !string.IsNullOrEmpty(objeto.ToString()))
			{
				long outValue;
				return long.TryParse(objeto.ToString(), NumberStyles.None, CultureInfo.CurrentCulture, out outValue) ? outValue : null;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Validar y setear el valor de un row tipo Double nullable
		/// </summary>
		/// <param name="objeto">Objeto row</param>
		/// <returns></returns>
		private static double? getRowDouble(object objeto)
		{
			if (objeto != null && !string.IsNullOrEmpty(objeto.ToString()))
			{
				double outValue;
				return double.TryParse(objeto.ToString(), NumberStyles.None, CultureInfo.CurrentCulture, out outValue) ? outValue : null;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Validar y setear el valor de un row tipo Float nullable
		/// </summary>
		/// <param name="objeto">Objeto row</param>
		/// <returns></returns>
		private static float? getRowFloat(object objeto)
		{
			if (objeto != null && !string.IsNullOrEmpty(objeto.ToString()))
			{
				float outValue;
				return float.TryParse(objeto.ToString(), NumberStyles.None, CultureInfo.CurrentCulture, out outValue) ? outValue : null;
			}
			else
			{
				return null;
			}
		}
	}
}
