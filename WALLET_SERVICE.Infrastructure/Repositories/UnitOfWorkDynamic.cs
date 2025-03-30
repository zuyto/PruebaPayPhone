// <copyright file="UnitOfWorkDynamic.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.EntityFrameworkCore.Infrastructure;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Infrastructure.Context;

namespace WALLET_SERVICE.Infrastructure.Repositories
{
	internal class UnitOfWorkDynamic : IUnitOfWorkDynamic
	{
		private readonly ISerilogImplements _serilogImplements;

		public UnitOfWorkDynamic(DynamicContext? dBContext, ISerilogImplements serilogImplements)
		{
			_DBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
			_serilogImplements = serilogImplements;
		}

		#region Repositories

		public IDynamicRepository DinamicRepository =>
			new DynamicRepository(_DBContext, _serilogImplements);

		#endregion Repositories

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void SaveChanges()
		{
			_DBContext.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _DBContext.SaveChangesAsync();
		}

		public DatabaseFacade Database => _DBContext.Database;

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_DBContext?.Dispose();
				}

				_disposed = true;
			}
		}

		#region Dependencias

		private bool _disposed;
		private readonly DynamicContext _DBContext;

		#endregion Dependencias
	}
}
