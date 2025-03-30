// <copyright file="RegexCustom.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace WALLET_SERVICE.Application.Common.Static
{
	[ExcludeFromCodeCoverage]
	public static partial class RegexCustom
	{
		/// <summary>
		/// Expresión regular para validar un separador (_) o (-) de números
		/// </summary>
		public static readonly Regex SeparadorRegex = new(@"^\d+(_|-)\d+$", RegexOptions.None);

		/// <summary>
		/// Expresión para validar ceros intermedios
		/// </summary>
		public static readonly Regex CerosIntermediosRegex = new(@"^\d{2,}0{4,}\d+$", RegexOptions.None);

		/// <summary>
		/// Expresión para validar secuencia repetitiva
		/// </summary>
		public static readonly Regex SecuenciaRepetitivaRegex = new(@"^(\d{2,})(.*\1.*)$", RegexOptions.None);

		/// <summary>
		/// Expresión para validar números no procesables
		/// </summary>
		public static readonly Regex NoProcesablesRegex = new(@"^(?:\d{6,}0{4,}\d+|\d{2,}0{4,}\d{2,}0{4,}\d+)$", RegexOptions.None);

		/// <summary>
		/// Expresión para eliminar ceros en el medio
		/// </summary>
		public static readonly Regex EliminaCerosMedio = new(@"^(\d{2})0+(\d+)$", RegexOptions.None);

		/// <summary>
		/// Expresión para validar caracteres alfanuméricos con guiones
		/// </summary>
		public static readonly Regex CaracteresAlfanumericosconGiones = new(@"^[0-9\-_]+$", RegexOptions.None);

		/// <summary>
		/// Expresión para validar si el string es de solo ceros
		/// </summary>
		public static readonly Regex SoloCeros = new(@"^0+$", RegexOptions.None);


	}
}
