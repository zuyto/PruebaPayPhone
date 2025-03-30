// <copyright file="WalletDbContext.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.EntityFrameworkCore;

using WALLET_SERVICE.Domain.Entities.Wallet;

namespace WALLET_SERVICE.Infrastructure.Context;

public partial class WalletDbContext : DbContext
{
	public WalletDbContext()
	{
	}

	public WalletDbContext(DbContextOptions<WalletDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Transaction> Transactions { get; set; }

	public virtual DbSet<Wallet> Wallets { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Transaction>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC070D41D5E8");

			entity.ToTable("Transaction");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.Type).HasMaxLength(10);

			entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
				.HasForeignKey(d => d.WalletId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Wallet_Transaction");
		});

		modelBuilder.Entity<Wallet>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07EF4FA8DC");

			entity.ToTable("Wallet");

			entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.DocumentId).HasMaxLength(20);
			entity.Property(e => e.Name).HasMaxLength(100);
			entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
