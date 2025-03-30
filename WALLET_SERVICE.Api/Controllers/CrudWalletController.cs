// <copyright file="CrudWalletController.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.AspNetCore.Mvc;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;

namespace WALLET_SERVICE.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CrudWalletController : ControllerBase
	{
		private readonly ISerilogImplements _serilogImplements;
		//private readonly ISerilogImplements _serilogImplements;
	}
}
