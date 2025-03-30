// <copyright file="IWalletTransferRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.Interfaces.Repository.Wallet
{
	public interface IWalletTransferRepository
	{
		Task<Domain.Entities.Wallet.Wallet?> GetByIdAsync(int walletId);
		Task<IEnumerable<Domain.Entities.Wallet.Wallet>> GetAllAsync();
		Task AddAsync(Domain.Entities.Wallet.Wallet wallet);
		Task UpdateAsync(Domain.Entities.Wallet.Wallet wallet);
		Task DeleteAsync(int walletId);
	}
}
