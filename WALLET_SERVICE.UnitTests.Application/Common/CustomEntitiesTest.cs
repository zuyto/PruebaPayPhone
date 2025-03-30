// <copyright file="CustomEntitiesTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using WALLET_SERVICE.Application.Common.CustomEntities;

namespace WALLET_SERVICE.UnitTests.Application.Common
{
	public class CustomEntitiesTest
	{
		private static readonly int[] primeraPagina = { 1, 2 };
		private static readonly int[] segundaPagina = { 3, 4 };
		private static readonly int[] terceraPagina = { 5 };

		public static IEnumerable<object[]> paginaDeDatos => new List<object[]>
		{
			new object[] { 1, 2, primeraPagina },
			new object[] { 2, 2, segundaPagina },
			new object[] { 3, 2, terceraPagina }
		};

		[Fact]
		public void Metadata_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var metadata = new Metadata();

			// Act
			metadata.TotalCount = 100;
			metadata.PageSize = 10;
			metadata.CurrentPage = 1;
			metadata.TotalPages = 10;
			metadata.HasNextPage = true;
			metadata.HasPreviousPage = false;
			metadata.NexPageUrl = "/api/resource?page=2";
			metadata.PreviousPageUrl = null;

			// Assert
			Assert.Equal(100, metadata.TotalCount);
			Assert.Equal(10, metadata.PageSize);
			Assert.Equal(1, metadata.CurrentPage);
			Assert.Equal(10, metadata.TotalPages);
			Assert.True(metadata.HasNextPage);
			Assert.False(metadata.HasPreviousPage);
			Assert.Equal("/api/resource?page=2", metadata.NexPageUrl);
			Assert.Null(metadata.PreviousPageUrl);
		}

		[Fact]
		public void PagedList_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var pagedList = new PagedList<int>();

			// Act
			pagedList.CurrentPage = 1;
			pagedList.TotalPages = 3;
			pagedList.PageSize = 10;
			pagedList.TotalCount = 30;

			// Assert
			Assert.Equal(1, pagedList.CurrentPage);
			Assert.Equal(3, pagedList.TotalPages);
			Assert.Equal(10, pagedList.PageSize);
			Assert.Equal(30, pagedList.TotalCount);
			Assert.False(pagedList.HasPreviousPage);
			Assert.True(pagedList.HasNextPage);
			Assert.Equal(2, pagedList.NextPageNumber);
			Assert.Null(pagedList.PreviousPageNumber);
		}

		[Fact]
		public void PagedList_Create_ReturnsPagedListWithCorrectProperties()
		{
			// Arrange
			var source = Enumerable.Range(1, 30); // 30 numbers
			var pageNumber = 2;
			var pageSize = 10;

			// Act
			var pagedList = PagedList<int>.Create(source, pageNumber, pageSize);

			// Assert
			Assert.Equal(2, pagedList.CurrentPage);
			Assert.Equal(3, pagedList.TotalPages);
			Assert.Equal(10, pagedList.PageSize);
			Assert.Equal(30, pagedList.TotalCount);
			Assert.True(pagedList.HasPreviousPage);
			Assert.True(pagedList.HasNextPage);
			Assert.Equal(3, pagedList.NextPageNumber);
			Assert.Equal(1, pagedList.PreviousPageNumber);
			Assert.True(pagedList.HasNextPage);
			Assert.True(pagedList.HasNextPage);
		}

		[Fact]
		public void PaginationOptions_PropertiesCanBeSetAndGet()
		{
			// Arrange
			var paginationOptions = new PaginationOptions();

			// Act
			paginationOptions.DefaultPageSize = 20;
			paginationOptions.DefaultPageNumber = 1;

			// Assert
			Assert.Equal(20, paginationOptions.DefaultPageSize);
			Assert.Equal(1, paginationOptions.DefaultPageNumber);
		}

		[Fact]
		public void Constructor_Sets_Properties()
		{
			// Arrange
			var items = new List<int> { 1, 2, 3 };
			int count = 3, pageNumber = 1, pageSize = 2;

			// Act
			var pagedList = new PagedList<int>(items, count, pageNumber, pageSize);

			// Assert
			Assert.Equal(count, pagedList.TotalCount);
			Assert.Equal(pageNumber, pagedList.CurrentPage);
			Assert.Equal(pageSize, pagedList.PageSize);
			Assert.Equal(2,
				pagedList.TotalPages); // Assuming count/pageSize rounds up
		}

		[Theory]
		[InlineData(1, 2,
			false)] // CurrentPage is 1, TotalPages is 2, HasPreviousPage should be false
		[InlineData(2, 2,
			true)] // CurrentPage is 2, TotalPages is 2, HasPreviousPage should be true
		public void HasPreviousPage_Returns_Expected_Value(int currentPage,
			int totalPages, bool expectedResult)
		{
			// Arrange
			var pagedList = new PagedList<int>
			{
				CurrentPage = currentPage,
				TotalPages = totalPages
			};

			// Act & Assert
			Assert.Equal(expectedResult, pagedList.HasPreviousPage);
		}

		[Theory]
		[InlineData(1, 2,
			true)] // CurrentPage is 1, TotalPages is 2, HasNextPage should be true
		[InlineData(2, 2,
			false)] // CurrentPage is 2, TotalPages is 2, HasNextPage should be false
		public void HasNextPage_Returns_Expected_Value(int currentPage,
			int totalPages, bool expectedResult)
		{
			// Arrange
			var pagedList = new PagedList<int>
			{
				CurrentPage = currentPage,
				TotalPages = totalPages
			};

			// Act & Assert
			Assert.Equal(expectedResult, pagedList.HasNextPage);
		}

		[Fact]
		public void Constructor_Adds_Items_To_List()
		{
			// Arrange
			var items = new List<int> { 1, 2, 3 };
			int count = 3, pageNumber = 1, pageSize = 2;

			// Act
			var pagedList = new PagedList<int>(items, count, pageNumber, pageSize);

			// Assert
			Assert.Equal(items.Count, pagedList.Count);
			Assert.All(items, item => Assert.Contains(item, pagedList));
		}

		[Fact]
		public void NextPageNumber_ReturnsCorrectValue_WhenHasNextPage()
		{
			// Arrange
			var pagedList =
				new PagedList<int>(new List<int>(), 10, 1,
					5); // TotalPages would be 2

			// Act
			var nextPageNumber = pagedList.NextPageNumber;

			// Assert
			Assert.Equal(2, nextPageNumber);
		}

		[Fact]
		public void NextPageNumber_ReturnsNull_WhenNoNextPage()
		{
			// Arrange
			var pagedList =
				new PagedList<int>(new List<int>(), 10, 2,
					5); // TotalPages would be 2

			// Act
			var nextPageNumber = pagedList.NextPageNumber;

			// Assert
			Assert.Null(nextPageNumber);
		}
		[Theory]
		[InlineData(1, 5, 5)] // Primera página, tamaño de página 5, debería obtener los primeros 5 elementos
		[InlineData(2, 5, 5)] // Segunda página, tamaño de página 5, debería obtener los siguientes 5 elementos
		[InlineData(1, 10, 10)] // Primera página, tamaño de página 10, debería obtener 10 elementos
		public void Create_ShouldReturnCorrectPageSizeAndPageNumber(int pageNumber, int pageSize, int expectedItemCount)
		{
			// Arrange
			var source = Enumerable.Range(1, 50); // Crear una fuente de datos de prueba

			// Act
			var pagedList = PagedList<int>.Create(source, pageNumber, pageSize);

			// Assert
			Assert.Equal(expectedItemCount, pagedList.Count); // Verificar el conteo de elementos en la página
			Assert.Equal(pageNumber, pagedList.CurrentPage); // Verificar que el número de página sea correcto
			Assert.Equal(pageSize, pagedList.PageSize); // Verificar que el tamaño de página sea correcto
		}

		[Fact]
		public void Create_ShouldHandleEmptySource()
		{
			// Arrange
			var source = Enumerable.Empty<int>(); // Fuente de datos vacía

			// Act
			var pagedList = PagedList<int>.Create(source, 1, 10);

			// Assert
			Assert.Empty(pagedList); // La lista paginada resultante debería estar vacía
		}

	}
}
