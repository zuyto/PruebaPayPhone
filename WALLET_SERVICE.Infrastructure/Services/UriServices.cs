// <copyright file="UriServices.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.Interfaces.Services;
using WALLET_SERVICE.Application.Common.QueryFilter;

namespace WALLET_SERVICE.Infrastructure.Services
{
	public class UriServices : IUriServices
	{
		private readonly string baseUri;

		public UriServices(string baseUri)
		{
			this.baseUri = baseUri;
		}

		public Uri GetUserPaginationUri(UserQueryFilter filter, string actionUrl)
		{
			var baseUrl = $"{baseUri}{actionUrl}";
			return new Uri(baseUrl);
		}
	}

}
