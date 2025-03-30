// <copyright file="DynamicRepository.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using WALLET_SERVICE.Application.Common.Interfaces.Repository;
using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Application.Common.Static;
using WALLET_SERVICE.Application.Common.Struct;
using WALLET_SERVICE.Infrastructure.Context;
using WALLET_SERVICE.Infrastructure.Extensions;

namespace WALLET_SERVICE.Infrastructure.Repositories
{
	[ExcludeFromCodeCoverage]
	internal class DynamicRepository : IDynamicRepository
	{
		private readonly DynamicContext context;
		private readonly ISerilogImplements _serilogImplements;

		public DynamicRepository(DynamicContext context, ISerilogImplements serilogImplements)
		{
			this.context = context;
			_serilogImplements = serilogImplements;
		}


		public async Task<List<object>> ExecuteSentenciaOnDatabase(string sentence,
			string secreto)
		{
			_serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information,
				MetodosMessage.pingSecretConexion, sentence, null);
			var results = context.DynamicListFromSql(sentence,
				secreto.Decrypt(), new Dictionary<string, object>()).ToList();
			return await Task.FromResult(results);
		}

		public async Task<bool> TestConnectionDynamic(string secreto)
		{
			var results = context.TestConnectionDynamic(secreto.Decrypt());
			return await Task.FromResult(results);
		}
	}
}
