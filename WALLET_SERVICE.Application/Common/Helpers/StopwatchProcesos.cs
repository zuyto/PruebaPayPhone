// <copyright file="StopwatchProcesos.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Application.Common.Helpers
{
	[ExcludeFromCodeCoverage]
	public class StopwatchProcesos
	{
		private readonly ISerilogImplements _serilogImplements;

		public StopwatchProcesos(ISerilogImplements serilogImplements)
		{
			_serilogImplements = serilogImplements;
		}

		/// <summary>
		/// Se encarga de iniciar un Stopwatch para evaluar la duracion de un proceso  e imprimir un log
		/// </summary>
		/// <param name="nombreProceso"> nombre del proceso a evaluar </param>
		/// <returns>un Stopwatch</returns>
		public virtual async Task<Stopwatch> InicioProceso(string nombreProceso)
		{
			Stopwatch tiempoProceso = new Stopwatch();
			_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, "", null, $"\n\n***** INICIO TIEMPO PROCESO {nombreProceso} ***** \n");
			await Task.Run(tiempoProceso.Start);

			return tiempoProceso;

		}

		/// <summary>
		/// Metodo encargado de Finalizar un Stopwatch en la duracion de un proceso e imprimir un log con el tiempo usado
		/// </summary>
		/// <param name="stopwatch">datos del StopWatch</param>
		/// <param name="nombreProceso">nombre del proceso finalizado</param>
		/// <returns></returns>
		public virtual async Task FinalizaProceso(Stopwatch stopwatch, string nombreProceso)
		{
			await Task.Run(stopwatch.Stop);
			TimeSpan timeTakenControl = stopwatch.Elapsed;

			DtoStopwatch tiempoProceso = new DtoStopwatch
			{
				Days = timeTakenControl.Days,
				Hours = timeTakenControl.Hours,
				Minutes = timeTakenControl.Minutes,
				Seconds = timeTakenControl.Seconds,
				Milliseconds = timeTakenControl.Milliseconds,
				TotalDays = timeTakenControl.TotalDays,
				TotalHours = timeTakenControl.TotalHours,
				TotalMilliseconds = timeTakenControl.TotalMilliseconds,
				TotalMinutes = timeTakenControl.TotalMinutes,
				TotalSeconds = timeTakenControl.TotalSeconds

			};

			_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(tiempoProceso, Formatting.Indented), null, $"\n\n***** FIN TIEMPO PROCESO {nombreProceso} ***** \n");

		}
	}
}
