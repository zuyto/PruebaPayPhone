// <copyright file="UnitOfWorkDynamicTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.EntityFrameworkCore.Infrastructure;

using Moq;

using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
using WALLET_SERVICE.Infrastructure.Context;
using WALLET_SERVICE.Infrastructure.Repositories;

namespace WALLET_SERVICE.Infrastructure.UnitTest.Repositories
{
	public class UnitOfWorkDynamicTest
	{
		private readonly Mock<DynamicContext> _mockContext;
		private readonly UnitOfWorkDynamic _unitOfWork;
		private readonly Mock<ISerilogImplements> _serilogImplements;

		public UnitOfWorkDynamicTest()
		{
			_serilogImplements = new Mock<ISerilogImplements>();
			_mockContext = new Mock<DynamicContext>();
			_unitOfWork = new UnitOfWorkDynamic(_mockContext.Object, _serilogImplements.Object);
		}

		[Fact]
		public void Constructor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
		{
			Assert.Throws<ArgumentNullException>(() => new UnitOfWorkDynamic(null, _serilogImplements.Object));
		}

		[Fact]
		public void SaveChanges_Should_Call_SaveChanges_On_Context()
		{
			_unitOfWork.SaveChanges();
			_mockContext.Verify(c => c.SaveChanges(), Times.Once);
		}

		[Fact]
		public async Task SaveChangesAsync_Should_Call_SaveChangesAsync_On_Context()
		{
			await _unitOfWork.SaveChangesAsync();
			_mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
		}

		[Fact]
		public void Dispose_Should_Call_Dispose_On_Context()
		{
			_unitOfWork.Dispose();
			_mockContext.Verify(c => c.Dispose(), Times.Once);
		}

		[Fact]
		public void Database_Should_Return_DatabaseFacade_From_Context()
		{
			var mockDatabaseFacade = new Mock<DatabaseFacade>(_mockContext.Object);
			_mockContext.Setup(c => c.Database).Returns(mockDatabaseFacade.Object);

			var databaseFacade = _unitOfWork.Database;

			Assert.Equal(mockDatabaseFacade.Object, databaseFacade);
		}
		[Fact]
		public void DinamicRepository_Should_Return_New_DynamicRepository()
		{
			var repository = _unitOfWork.DinamicRepository;
			Assert.NotNull(repository);
			Assert.IsType<DynamicRepository>(repository);
		}
		[Fact]
		public void Dispose_Should_Set_Disposed_To_True()
		{
			_unitOfWork.Dispose();
			var fieldInfo = _unitOfWork.GetType().GetField("_disposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(fieldInfo);
			var disposedValue = fieldInfo.GetValue(_unitOfWork) as bool?;
			Assert.True(disposedValue.HasValue && disposedValue.Value);
		}

		[Fact]
		public void Dispose_Called_Multiple_Times_Should_Not_Throw()
		{
			_unitOfWork.Dispose();
			var exception = Record.Exception(() => _unitOfWork.Dispose());
			Assert.Null(exception);
		}
		[Fact]
		public void Constructor_Should_Initialize_SerilogImplements()
		{
			var mockSerilog = new Mock<ISerilogImplements>();
			var unitOfWork = new UnitOfWorkDynamic(_mockContext.Object, mockSerilog.Object);

			var fieldInfo = unitOfWork.GetType().GetField("_serilogImplements", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(fieldInfo);
			var serilogValue = fieldInfo.GetValue(unitOfWork) as ISerilogImplements;
			Assert.Equal(mockSerilog.Object, serilogValue);
		}

		[Fact]
		public void DinamicRepository_Should_Use_SerilogImplements()
		{
			var mockSerilog = new Mock<ISerilogImplements>();
			var unitOfWork = new UnitOfWorkDynamic(_mockContext.Object, mockSerilog.Object);

			var repository = unitOfWork.DinamicRepository;
			var fieldInfo = repository.GetType().GetField("_serilogImplements", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(fieldInfo);
			var serilogValue = fieldInfo.GetValue(repository) as ISerilogImplements;
			Assert.Equal(mockSerilog.Object, serilogValue);
		}

	}
}
