// <copyright file="WalletServiceTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using AutoFixture;

using Moq;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Services;
using WALLET_SERVICE.Application.Common.Models.DTOs;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Services;
using WALLET_SERVICE.Domain.Entities.Wallet;

public class WalletServiceTests
{
	private readonly IFixture _fixture;
	private readonly Mock<IUnitOfWorkWalletTransfer> _unitOfWorkMock;
	private readonly IWalletService _walletService;

	public WalletServiceTests()
	{
		_fixture = new Fixture();
		_unitOfWorkMock = new Mock<IUnitOfWorkWalletTransfer>();
		_walletService = new WalletService(_unitOfWorkMock.Object);
	}

	[Fact]
	public async Task TransferAsync_ShouldReturnError_WhenAmountIsZeroOrNegative()
	{
		// Arrange
		var request = _fixture.Build<DtoTransferJsonRequest>()
			.With(r => r.Amount, 0)
			.Create();

		// Act
		var result = await _walletService.TransferAsync(request);

		// Assert
		Assert.False(result.EsExitoso);
		Assert.Equal(UserTypeMessages.ERROR_CERO_MONTO, result.Resultado.Mensaje);
	}

	[Fact]
	public async Task TransferAsync_ShouldReturnError_WhenFromWalletDoesNotExist()
	{
		// Arrange
		var request = _fixture.Create<DtoTransferJsonRequest>();
		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.FromWalletId))
			.ReturnsAsync((Wallet)null);

		// Act
		var result = await _walletService.TransferAsync(request);

		// Assert
		Assert.False(result.EsExitoso);
		Assert.Equal(UserTypeMessages.ERROR_WALLET_ORIGEN_INVALIDO, result.Resultado.Mensaje);
	}

	[Fact]
	public async Task TransferAsync_ShouldReturnError_WhenToWalletDoesNotExist()
	{
		// Arrange
		var request = _fixture.Create<DtoTransferJsonRequest>();
		var fromWallet = _fixture.Create<Wallet>();

		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.FromWalletId))
			.ReturnsAsync(fromWallet);
		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.ToWalletId))
			.ReturnsAsync((Wallet)null);

		// Act
		var result = await _walletService.TransferAsync(request);

		// Assert
		Assert.False(result.EsExitoso);
		Assert.Equal(UserTypeMessages.ERROR_WALLET_DESTINO_INVALIDO, result.Resultado.Mensaje);
	}

	[Fact]
	public async Task TransferAsync_ShouldReturnError_WhenInsufficientBalance()
	{
		// Arrange
		var request = _fixture.Create<DtoTransferJsonRequest>();
		var fromWallet = _fixture.Build<Wallet>().With(w => w.Balance, request.Amount - 1).Create();
		var toWallet = _fixture.Create<Wallet>();

		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.FromWalletId))
			.ReturnsAsync(fromWallet);
		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.ToWalletId))
			.ReturnsAsync(toWallet);

		// Act
		var result = await _walletService.TransferAsync(request);

		// Assert
		Assert.False(result.EsExitoso);
		Assert.Equal(UserTypeMessages.SALDO_INSUFICIENTE, result.Resultado.Mensaje);
	}

	[Fact]
	public async Task TransferAsync_ShouldReturnSuccess_WhenTransferIsValid()
	{
		// Arrange
		var request = _fixture.Create<DtoTransferJsonRequest>();
		var fromWallet = _fixture.Build<Wallet>().With(w => w.Balance, request.Amount + 100).Create();
		var toWallet = _fixture.Create<Wallet>();

		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.FromWalletId))
			.ReturnsAsync(fromWallet);
		_unitOfWorkMock.Setup(u => u.WalletTransferRepository.GetByIdAsync(request.ToWalletId))
			.ReturnsAsync(toWallet);
		_unitOfWorkMock.Setup(u => u.SaveChangesAsync());

		// Act
		var result = await _walletService.TransferAsync(request);

		// Assert
		Assert.True(result.EsExitoso);
		Assert.Equal(UserTypeMessages.OKGEN01, result.Mensaje);
	}
}
