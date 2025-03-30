// <copyright file="TransactionRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using WALLET_SERVICE.Application.Common.Helpers;
using WALLET_SERVICE.Application.Common.Interfaces.Repository.Wallet;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Domain.Entities.Wallet;
using WALLET_SERVICE.Infrastructure.Context;

namespace WALLET_SERVICE.Infrastructure.Repositories.Wallet
{
	public class TransactionRepository(WalletDbContext walletDbContext, ISerilogImplements serilogImplements) : ITransactionRepository
	{
		public readonly WalletDbContext _walletDbContext = walletDbContext;
		private readonly ISerilogImplements _serilogImplements = serilogImplements;

		public async Task AddAsync(Transaction transf)
		{
			try
			{
				await _walletDbContext.Transactions.AddAsync(transf);
				await _walletDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR AddAsync **** \n");
				throw;
			}
		}

		public async Task DeleteAsync(int transfId)
		{
			try
			{
				var wallet = await _walletDbContext.Transactions.FindAsync(transfId);
				if (wallet != null)
				{
					_walletDbContext.Transactions.Remove(wallet);
					await _walletDbContext.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR DeleteAsync **** \n");

			}
		}

		public async Task<IEnumerable<Transaction>> GetAllAsync()
		{
			try
			{
				return await _walletDbContext.Transactions.ToListAsync();
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR GetAllAsync **** \n");
				throw;
			}
		}

		public async Task<Transaction?> GetByIdAsync(int transfId)
		{
			try
			{
				return await _walletDbContext.Transactions.FindAsync(transfId);
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR GetByIdAsync **** \n");
				throw;
			}
		}

		public async Task UpdateAsync(Transaction transf)
		{
			try
			{
				_walletDbContext.Transactions.Update(transf);
				await _walletDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** CATCH ERROR UpdateAsync **** \n");

			}
		}
	}
}
