// <copyright file="QueryFilterTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.QueryFilter;

namespace WALLET_SERVICE.UnitTests.Application.Common
{
	public class QueryFilterTest
	{
		[Fact]
		public void ApplicationUserQueryFilter_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var filter = new ApplicationUserQueryFilter();

			// Act
			filter.IdAplicacion = 1;
			filter.FechaInicio = DateTime.Now;
			filter.FechaFin = DateTime.Now.AddHours(1);

			// Assert
			Assert.Equal(1, filter.IdAplicacion);
			Assert.NotNull(filter.FechaInicio);
			Assert.NotNull(filter.FechaFin);
		}

		[Fact]
		public void UserQueryFilter_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var filter = new UserQueryFilter();

			// Act
			filter.fechaInicio = DateTime.Now;
			filter.fechaFin = DateTime.Now.AddHours(1);
			filter.pageSize = 10;
			filter.pageNumber = 1;

			// Assert
			Assert.NotNull(filter.fechaInicio);
			Assert.NotNull(filter.fechaFin);
			Assert.Equal(10, filter.pageSize);
			Assert.Equal(1, filter.pageNumber);
		}
	}
}
