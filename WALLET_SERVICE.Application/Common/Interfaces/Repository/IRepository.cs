// <copyright file="IRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Linq.Expressions;

namespace WALLET_SERVICE.Application.Common.Interfaces.Repository
{
	public interface IRepository<T> where T : class
	{
		Task<List<T>> GetAll();
		Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
		Task<T?> GetById(int id);
		Task<T?> Get(Expression<Func<T, bool>> predicate);
		Task Add(T entity);
		void Update(T entity);
		Task Delete(int id);
		Task<int> CountRecord();
	}
}
