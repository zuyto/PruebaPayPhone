// <copyright file="Wallet.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Domain.Entities.Wallet;

public partial class Wallet
{
	public int Id { get; set; }

	public string DocumentId { get; set; } = null!;

	public string Name { get; set; } = null!;

	public decimal Balance { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime UpdatedAt { get; set; }

	public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
