// <copyright file="WalletController.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using WALLET_SERVICE.Api.Response;
using WALLET_SERVICE.Application.Common.Helpers;
using WALLET_SERVICE.Application.Common.Interfaces.Services;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Models.DTOs;
using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Common.Struct;

namespace WALLET_SERVICE.Api.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class WalletController : ControllerBase
	{
		private readonly ISerilogImplements _serilogImplements;
		private readonly IWalletService _application;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serilogImplements"></param>
		/// <param name="walletService"></param>
		public WalletController(ISerilogImplements serilogImplements, IWalletService walletService)
		{
			_serilogImplements = serilogImplements;
			_application = walletService;
		}

		/// <summary>
		/// Endpoint 
		/// </summary>
		/// <returns></returns>
		/// <response code="200">OK. Devuelve el objeto solicitado</response>
		/// <response code="409">Error durante el proceso</response>
		/// <response code="500">Error interno en el API</response>
		/// <response code="404">Error controlado cuando el Request es invalido</response>
		/// <response code="400">Error controlado por el flitro del request</response>
		[Route("Transfer")]
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<DtoTransferJsonResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<DtoTransferJsonResponse>), StatusCodes.Status409Conflict)]
		[ProducesResponseType(typeof(ApiResponse<DtoErrorResponse>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<DtoTransferJsonResponse>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Transfer([FromBody] DtoTransferJsonRequest request)
		{
			ObjectResult result;
			string methodName = nameof(Transfer);

			try
			{
				if (request == null)
				{
					return StatusCode(StatusCodes.Status404NotFound, ApiResponse<DtoTransferJsonResponse>.CreateError(UserTypeMessages.ERROR_REQUEST, new DtoTransferJsonResponse()));
				}

				DtoGenericResponse<DtoTransferJsonResponse> response = await _application.TransferAsync(request);


				if (!response.EsExitoso)
				{
					result = StatusCode(StatusCodes.Status409Conflict, ApiResponse<DtoTransferJsonResponse>.CreateUnsuccessful(response.Resultado!, response.Mensaje!));
				}
				else
				{
					result = Ok(ApiResponse<DtoTransferJsonResponse>.CreateSuccessful(response.Resultado!, response.Mensaje!));
				}

				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(result, Formatting.Indented), null, string.Format(UserTypeMessages.CONTROLLER_RESPONSE, methodName));

				return result;

			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Critical, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, string.Format(UserTypeMessages.ERRCATHCONTROLLER, methodName));
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<DtoErrorResponse>.CreateError(UserTypeMessages.ERROR_NO_CONTROLADO, ex.HandleExceptionMessage()));
			}
		}
	}
}
