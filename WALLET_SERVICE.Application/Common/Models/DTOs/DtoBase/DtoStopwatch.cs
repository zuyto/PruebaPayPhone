// <copyright file="DtoStopwatch.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase
{
	[ExcludeFromCodeCoverage]
	public class DtoStopwatch
	{
		public int Days { get; set; }
		public int Hours { get; set; }
		public int Milliseconds { get; set; }
		public int Minutes { get; set; }
		public int Seconds { get; set; }
		public double TotalDays { get; set; }
		public double TotalHours { get; set; }
		public double TotalMilliseconds { get; set; }
		public double TotalMinutes { get; set; }
		public double TotalSeconds { get; set; }
	}
}
