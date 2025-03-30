// <copyright file="BuildOracleSql.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Oracle.ManagedDataAccess.Client;

namespace WALLET_SERVICE.Infrastructure.Extensions
{
	internal static class BuildOracleSql
	{
		/// <summary>
		///     Construir sentencia inicial y final para las funciones o procedimiento
		///     almacenado (BEGIN END)
		/// </summary>
		/// <param name="fn"></param>
		/// <returns></returns>
		public static string BuildSqlFunction(this string fn)
		{
			return $"SELECT * FROM {fn}";
		}

		/// <summary>
		///     Construir sentencia inicial y final para las funciones o procedimiento
		///     almacenado (BEGIN END)
		/// </summary>
		/// <param name="fn"></param>
		/// <returns></returns>
		public static string BuildSqlSP(this string fn)
		{
			return $"CALL {fn}";
		}

		/// <summary>
		///     Obtener datos dinamicos dado una cadena de conexion
		/// </summary>
		/// <param name="db">db conectext</param>
		/// <param name="Sql">sentencia sql</param>
		/// <param name="connectionString">cadena de conexion</param>
		/// <param name="Params"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public static IEnumerable<dynamic> DynamicListFromSql(this DbContext db,
			string Sql, string connectionString,
			Dictionary<string, object> Params)
		{
			using var connection = db.Database.GetDbConnection();
			using var cmd = connection.CreateCommand();
			cmd.CommandText = Sql;
			cmd.Connection = connection;
			cmd.Connection.ConnectionString = connectionString;
			cmd.CommandTimeout = 1800; //30 minutos

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}

			foreach (var p in Params)
			{
				var dbParameter = cmd.CreateParameter();
				dbParameter.ParameterName = p.Key;
				dbParameter.Value = p.Value;
				cmd.Parameters.Add(dbParameter);
			}

			using var dataReader = cmd.ExecuteReader();
			while (dataReader.Read())
			{
				var row = new ExpandoObject() as IDictionary<string, object>;
				for (var fieldCount = 0;
					 fieldCount < dataReader.FieldCount;
					 fieldCount++)
				{
					if (row.ContainsKey(dataReader.GetName(fieldCount)))
					{
						var columnName = dataReader.GetName(fieldCount)
							.RenameValue(row);
						row.Add(columnName, dataReader[fieldCount]);
					}
					else
					{
						row.Add(dataReader.GetName(fieldCount),
							dataReader[fieldCount]);
					}
				}

				yield return row;
			}

			connection.Close();
		}

		/// <summary>
		///     Obtener datos dinamicos
		/// </summary>
		/// <param name="db"></param>
		/// <param name="Sql"></param>
		/// <param name="Params"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public static IEnumerable<dynamic> DynamicListFromSql(this DbContext db,
			string Sql, Dictionary<string, object> Params)
		{
			using var cmd = db.Database.GetDbConnection().CreateCommand();

			cmd.CommandText = Sql;
			if (cmd.Connection != null &&
				cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}

			foreach (var p in Params)
			{
				var dbParameter = cmd.CreateParameter();
				dbParameter.ParameterName = p.Key;
				dbParameter.Value = p.Value;
				cmd.Parameters.Add(dbParameter);
			}

			using var dataReader = cmd.ExecuteReader();
			while (dataReader.Read())
			{
				var row = new ExpandoObject() as IDictionary<string, object>;
				for (var fieldCount = 0;
					 fieldCount < dataReader.FieldCount;
					 fieldCount++)
				{
					row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
				}

				yield return row;
			}
		}

		/// <summary>
		///     Prueba de conexión
		/// </summary>
		/// <param name="db"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public static bool TestConnectionDynamic(this DbContext db,
			string connectionString)
		{
			var connection = db.Database.GetDbConnection();

			try
			{
				using var cmd = connection.CreateCommand();
				if (cmd.Connection != null)
				{
					cmd.Connection.ConnectionString = connectionString;
					cmd.Connection.Open();

					return cmd.Connection.State == ConnectionState.Open;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
			finally
			{
				connection.Close();
			}
		}
		[ExcludeFromCodeCoverage]
		private static string RenameValue(this string name,
			IDictionary<string, object> dictionary, int i = 1)
		{
			return dictionary.ContainsKey($"{name}_{i}")
				? RenameValue(name, dictionary, i + 1)
				: $"{name}_{i}";
		}

		/// <summary>
		///     Ejecutar procedimiento almacenado oracle
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="db"></param>
		/// <param name="sql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public static IQueryable<TEntity> ExecuteStoredProcedure<TEntity>(
			this DbSet<TEntity> source, string sql,
			params OracleParameter[] parameters) where TEntity : class
		{
			return source.FromSqlRaw(sql, parameters);
		}

		/// <summary>
		///     Construir los parametros para las funciones de Oracle
		/// </summary>
		/// <param name="fn">función</param>
		/// <param name="parameters">Listado de parametros</param>
		/// <returns></returns>
		public static string BuildParameters(this string fn,
			OracleParameter[] parameters, bool includeOutput = false)
		{
			var paramsfilter = parameters;

			if (!includeOutput)
			{
				paramsfilter = parameters
					.Where(s => s.Direction != ParameterDirection.Output).ToArray();
			}

			StringBuilder parametersString = new();
			foreach (var parameter in paramsfilter)
			{
				parametersString.Append((parametersString.Length > 0 ? ",:" : ":") +
										parameter.ParameterName);
			}

			return $"{fn}({parametersString});";
		}

		/// <summary>
		///     Construir sentencia inicial y final para las funciones o procedimiento
		///     almacenado (BEGIN END)
		/// </summary>
		/// <param name="fn"></param>
		/// <returns></returns>
		public static string BuildSql(this string fn)
		{
			return $"BEGIN {fn} END;";
		}
	}

}
