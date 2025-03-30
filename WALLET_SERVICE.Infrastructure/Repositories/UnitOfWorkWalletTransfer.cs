// <copyright file="UnitOfWorkWalletTransfer.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Repository.Wallet;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Infrastructure.Context;
using WALLET_SERVICE.Infrastructure.Repositories.Wallet;

namespace WALLET_SERVICE.Infrastructure.Repositories
{
	[ExcludeFromCodeCoverage]
	public class UnitOfWorkWalletTransfer : IUnitOfWorkWalletTransfer
	{
		private readonly WalletDbContext _walletDbContext;
		private readonly ISerilogImplements _serilogImplements;

		public UnitOfWorkWalletTransfer(WalletDbContext hcinvSglDbContext, ISerilogImplements serilogImplements)
		{
			_walletDbContext = hcinvSglDbContext;
			_serilogImplements = serilogImplements;
		}

		public IWalletTransferRepository WalletTransferRepository => new WalletTransferRepository(_walletDbContext, _serilogImplements);
		public ITransactionRepository TransactionRepository => new TransactionRepository(_walletDbContext, _serilogImplements);

		public void SaveChanges()
		{
			_walletDbContext.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _walletDbContext.SaveChangesAsync();
		}
	}

}
