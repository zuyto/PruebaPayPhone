// <copyright file="WalletControllerTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>
using AutoFixture;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using WALLET_SERVICE.Api.Controllers;
using WALLET_SERVICE.Api.Response;
using WALLET_SERVICE.Application.Common.Interfaces.Services;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Models.DTOs;
using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;

namespace WalletService.Tests.Controllers
{
	public class WalletControllerTests
	{
		private readonly Mock<ISerilogImplements> _mockLogger;
		private readonly Mock<IWalletService> _mockWalletService;
		private readonly WalletController _controller;
		private readonly Fixture _fixture;

		public WalletControllerTests()
		{
			_mockLogger = new Mock<ISerilogImplements>();
			_mockWalletService = new Mock<IWalletService>();
			_controller = new WalletController(_mockLogger.Object, _mockWalletService.Object);
			_fixture = new Fixture();
		}

		[Fact]
		public async Task Transfer_ShouldReturnOk_WhenTransferIsSuccessful()
		{
			// Arrange
			var request = _fixture.Create<DtoTransferJsonRequest>();
			var response = _fixture.Build<DtoGenericResponse<DtoTransferJsonResponse>>()
								   .With(r => r.EsExitoso, true)
								   .Create();

			_mockWalletService.Setup(s => s.TransferAsync(request)).ReturnsAsync(response);

			// Act
			var result = await _controller.Transfer(request);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var apiResponse = Assert.IsType<ApiResponse<DtoTransferJsonResponse>>(okResult.Value);
			Assert.True(apiResponse.IsSuccessful);
		}

		[Fact]
		public async Task Transfer_ShouldReturnConflict_WhenTransferFails()
		{
			// Arrange
			var request = _fixture.Create<DtoTransferJsonRequest>();
			var response = _fixture.Build<DtoGenericResponse<DtoTransferJsonResponse>>()
								   .With(r => r.EsExitoso, false)
								   .Create();

			_mockWalletService.Setup(s => s.TransferAsync(request)).ReturnsAsync(response);

			// Act
			var result = await _controller.Transfer(request);

			// Assert
			var conflictResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
		}

		[Fact]
		public async Task Transfer_ShouldReturnNotFound_WhenRequestIsNull()
		{
			// Act
			var result = await _controller.Transfer(null);

			// Assert
			var notFoundResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
		}

		[Fact]
		public async Task Transfer_ShouldReturnInternalServerError_OnException()
		{
			// Arrange
			var request = _fixture.Create<DtoTransferJsonRequest>();
			_mockWalletService.Setup(s => s.TransferAsync(request)).ThrowsAsync(new System.Exception("Internal error"));

			// Act
			var result = await _controller.Transfer(request);

			// Assert
			var errorResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status500InternalServerError, errorResult.StatusCode);
		}
	}
}
