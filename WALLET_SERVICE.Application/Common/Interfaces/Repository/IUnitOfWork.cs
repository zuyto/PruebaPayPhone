// <copyright file="IUnitOfWork.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WALLET_SERVICE.Application.Common.Interfaces.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		DatabaseFacade Database { get; }
		void SaveChanges();
		Task<int> SaveChangesAsync();
	}
}
