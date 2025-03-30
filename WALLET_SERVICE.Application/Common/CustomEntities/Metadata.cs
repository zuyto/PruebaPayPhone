// <copyright file="Metadata.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

namespace WALLET_SERVICE.Application.Common.CustomEntities
{
	public class Metadata
	{
		public int TotalCount { get; set; }

		public int PageSize { get; set; }

		public int CurrentPage { get; set; }

		public int TotalPages { get; set; }

		public bool HasNextPage { get; set; }

		public bool HasPreviousPage { get; set; }

		public string? NexPageUrl { get; set; }

		public string? PreviousPageUrl { get; set; }
	}
}
