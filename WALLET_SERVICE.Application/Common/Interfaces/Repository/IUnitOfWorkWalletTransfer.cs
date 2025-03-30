// <copyright file="IUnitOfWorkWalletTransfer.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Interfaces.Repository.Wallet;

namespace WALLET_SERVICE.Application.Common.Interfaces.Repository
{
	public interface IUnitOfWorkWalletTransfer
	{
		void SaveChanges();
		Task<int> SaveChangesAsync();

		#region[REPOSITORIES]
		public IWalletTransferRepository WalletTransferRepository { get; }
		public ITransactionRepository TransactionRepository { get; }
		#endregion
	}
}
