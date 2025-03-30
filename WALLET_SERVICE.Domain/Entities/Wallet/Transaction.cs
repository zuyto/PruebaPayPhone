// <copyright file="Transaction.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Domain.Entities.Wallet;

public partial class Transaction
{
	public int Id { get; set; }

	public int WalletId { get; set; }

	public decimal Amount { get; set; }

	public string Type { get; set; } = null!;

	public DateTime CreatedAt { get; set; }

	public virtual Wallet Wallet { get; set; } = null!;
}
