// <copyright file="AutoMapperTransversales.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

using AutoMapper;

using WALLET_SERVICE.Application.Common.Profiles;

namespace WALLET_SERVICE.Application.Common.Transversales
{
	[ExcludeFromCodeCoverage]
	public static class AutoMapperTransversales
	{

		////public static List<MandatoIncidenciaCmEnc> MapperDatosCierrePedidoProd(List<TblSglMandatoIncidenciaCmEnc> listaDatosCierrePedido)
		////{

		////    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));

		////    return mapper.Map<List<TblSglMandatoIncidenciaCmEnc>, List<MandatoIncidenciaCmEnc>>(listaDatosCierrePedido);

		////}

		////public static List<MandatoIncidenciaCmDet> MapperDatosCierrePedidoProdDtl(List<TblSglMandatoIncidenciaCmDet> listaDatosCierrePedido)
		////{

		////    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));

		////    return mapper.Map<List<TblSglMandatoIncidenciaCmDet>, List<MandatoIncidenciaCmDet>>(listaDatosCierrePedido);

		////}

		public static List<TDestination> MapperGenericListToList<TSource, TDestination>(List<TSource> sourceList)
		{
			var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
			return mapper.Map<List<TSource>, List<TDestination>>(sourceList);
		}

		public static TDestination MapperGenericObjToObj<TSource, TDestination>(TSource sourceList)
		{
			var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
			return mapper.Map<TSource, TDestination>(sourceList);
		}

	}
}
