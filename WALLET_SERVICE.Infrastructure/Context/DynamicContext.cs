// <copyright file="DynamicContext.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

namespace WALLET_SERVICE.Infrastructure.Context
{
	[ExcludeFromCodeCoverage]
	public class DynamicContext : DbContext
	{
		public DynamicContext()
		{
		}

		public DynamicContext(DbContextOptions<DynamicContext> options) :
			base(options)
		{
		}

		protected override void OnConfiguring(
			DbContextOptionsBuilder optionsBuilder)
		{
			// Method intentionally left empty.
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Method intentionally left empty.
		}
	}
}
