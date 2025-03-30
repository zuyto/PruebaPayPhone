// <copyright file="WalletTransferRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Helpers;
using WALLET_SERVICE.Application.Common.Interfaces.Repository.Wallet;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Infrastructure.Context;

namespace WALLET_SERVICE.Infrastructure.Repositories.Wallet
{
	[ExcludeFromCodeCoverage]
	internal class WalletTransferRepository(WalletDbContext walletDbContext, ISerilogImplements serilogImplements) : IWalletTransferRepository
	{
		public readonly WalletDbContext _walletDbContext = walletDbContext;
		private readonly ISerilogImplements _serilogImplements = serilogImplements;

		public async Task AddAsync(Domain.Entities.Wallet.Wallet wallet)
		{
			try
			{
				await _walletDbContext.Wallets.AddAsync(wallet);
				await _walletDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR AddAsync **** \n");
				throw;
			}

		}

		public async Task DeleteAsync(int walletId)
		{
			try
			{
				var wallet = await _walletDbContext.Wallets.FindAsync(walletId);
				if (wallet != null)
				{
					_walletDbContext.Wallets.Remove(wallet);
					await _walletDbContext.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR DeleteAsync **** \n");
				throw;
			}

		}

		public async Task<IEnumerable<Domain.Entities.Wallet.Wallet>> GetAllAsync()
		{
			try
			{
				return await _walletDbContext.Wallets.ToListAsync();
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR GetAllAsync **** \n");
				throw;
			}

		}

		public async Task<Domain.Entities.Wallet.Wallet?> GetByIdAsync(int walletId)
		{
			try
			{
				return await _walletDbContext.Wallets.FindAsync(walletId);
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR GetByIdAsync **** \n");
				throw;
			}

		}


		public async Task UpdateAsync(Domain.Entities.Wallet.Wallet wallet)
		{
			try
			{
				_walletDbContext.Wallets.Update(wallet);
				await _walletDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR UpdateAsync **** \n");
				throw;
			}

		}

	}
}
