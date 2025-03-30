// <copyright file="IUriServices.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.QueryFilter;

namespace WALLET_SERVICE.Application.Common.Interfaces.Services
{
	public interface IUriServices
	{
		Uri GetUserPaginationUri(UserQueryFilter filter, string actionUrl);
	}
}
