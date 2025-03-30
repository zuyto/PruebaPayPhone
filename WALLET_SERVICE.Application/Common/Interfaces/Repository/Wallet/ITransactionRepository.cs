// <copyright file="ITransactionRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.Interfaces.Repository.Wallet
{
	public interface ITransactionRepository
	{
		Task<Domain.Entities.Wallet.Transaction?> GetByIdAsync(int transfId);
		Task<IEnumerable<Domain.Entities.Wallet.Transaction>> GetAllAsync();
		Task AddAsync(Domain.Entities.Wallet.Transaction transf);
		Task UpdateAsync(Domain.Entities.Wallet.Transaction transf);
		Task DeleteAsync(int transfId);

	}
}
