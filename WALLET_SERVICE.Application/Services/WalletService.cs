// <copyright file="WalletService.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Helpers;
using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Services;
using WALLET_SERVICE.Application.Common.Models.DTOs;
using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Domain.Entities.Wallet;

namespace WALLET_SERVICE.Application.Services
{
	public class WalletService(IUnitOfWorkWalletTransfer unitOfWorkWalletTransfer) : IWalletService
	{
		private readonly IUnitOfWorkWalletTransfer _unitOfWorkWalletTransfer = unitOfWorkWalletTransfer;
		public async Task<DtoGenericResponse<DtoTransferJsonResponse>> TransferAsync(DtoTransferJsonRequest request)
		{

			if (request.Amount <= 0)
				return GenericHelpers.BuildResponse(false, new DtoTransferJsonResponse { Mensaje = UserTypeMessages.ERROR_CERO_MONTO }, UserTypeMessages.OKERRGEN01);


			Wallet fromWallet = await _unitOfWorkWalletTransfer.WalletTransferRepository.GetByIdAsync(request.FromWalletId);

			if (fromWallet == null)
				return GenericHelpers.BuildResponse(false, new DtoTransferJsonResponse { Mensaje = UserTypeMessages.ERROR_WALLET_ORIGEN_INVALIDO }, UserTypeMessages.OKERRGEN01);

			Wallet toWallet = await _unitOfWorkWalletTransfer.WalletTransferRepository.GetByIdAsync(request.ToWalletId);
			if (toWallet == null)
				return GenericHelpers.BuildResponse(false, new DtoTransferJsonResponse { Mensaje = UserTypeMessages.ERROR_WALLET_DESTINO_INVALIDO }, UserTypeMessages.OKERRGEN01);

			if (fromWallet.Balance < request.Amount)
			{
				return GenericHelpers.BuildResponse(false, new DtoTransferJsonResponse { Mensaje = UserTypeMessages.SALDO_INSUFICIENTE }, UserTypeMessages.OKERRGEN01);
			}



			fromWallet.Balance -= request.Amount;
			fromWallet.UpdatedAt = DateTime.UtcNow;



			var debitTransaction = new Transaction
			{
				WalletId = fromWallet.Id,
				Amount = request.Amount,
				Type = "Debit",
				CreatedAt = DateTime.UtcNow
			};

			toWallet.Balance += request.Amount;
			toWallet.UpdatedAt = DateTime.UtcNow;

			var creditTransaction = new Transaction
			{
				WalletId = toWallet.Id,
				Amount = request.Amount,
				Type = "Credit",
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWorkWalletTransfer.SaveChangesAsync();


			await _unitOfWorkWalletTransfer.TransactionRepository.UpdateAsync(debitTransaction);
			await _unitOfWorkWalletTransfer.TransactionRepository.UpdateAsync(creditTransaction);

			return GenericHelpers.BuildResponse<DtoTransferJsonResponse>(true, null, UserTypeMessages.OKGEN01);


		}
	}
}
