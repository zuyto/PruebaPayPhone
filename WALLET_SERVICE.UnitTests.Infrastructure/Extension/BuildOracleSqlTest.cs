// <copyright file="BuildOracleSqlTest.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using System.Data;

using Oracle.ManagedDataAccess.Client;

using WALLET_SERVICE.Infrastructure.Extensions;

namespace WALLET_SERVICE.UnitTests.Infrastructure.Extension
{
	public class BuildOracleSqlTest
	{
		[Theory]
		[InlineData("myFunction", "SELECT * FROM myFunction")]
		public void BuildSqlFunction_ShouldReturnCorrectSql(string input,
			string expected)
		{
			var result = input.BuildSqlFunction();
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("myProcedure", "CALL myProcedure")]
		public void BuildSqlSP_ShouldReturnCorrectSql(string input, string expected)
		{
			var result = input.BuildSqlSP();
			Assert.Equal(expected, result);
		}

		[Fact]
		public void BuildParameters_ShouldReturnCorrectSql()
		{
			// Configura tus parámetros de Oracle aquí
			var inParam1 = new OracleParameter("P_TRANSACCION",
				OracleDbType.Varchar2, ParameterDirection.Input)
			{ Value = "" };
			var inParam2 =
				new OracleParameter("P_OC", OracleDbType.Varchar2,
					ParameterDirection.Input)
				{ Value = "" };
			var inParam3 = new OracleParameter("P_ASN", OracleDbType.Varchar2,
				ParameterDirection.Input)
			{ Value = "" };
			var inParam4 = new OracleParameter("P_CITA", OracleDbType.Varchar2,
				ParameterDirection.Input)
			{ Value = "" };
			var inParam5 = new OracleParameter("P_USUARIO", OracleDbType.Varchar2,
				ParameterDirection.Input)
			{ Value = "" };

			//parametros salida
			var outParam = new OracleParameter("P_SALIDA", OracleDbType.RefCursor,
				ParameterDirection.Output);

			OracleParameter[] parameters =
			{
				inParam1, inParam2, inParam3, inParam4, inParam5, outParam
			};

			var functionName = "myFunction";
			var expected =
				"myFunction(:P_TRANSACCION,:P_OC,:P_ASN,:P_CITA,:P_USUARIO);";

			var result = functionName.BuildParameters(parameters);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("myFunction", "BEGIN myFunction END;")]
		public void BuildSql_ShouldReturnCorrectSql(string input, string expected)
		{
			var result = input.BuildSql();
			Assert.Equal(expected, result);
		}

		//[Fact]
		//   public void DynamicListFromSql_ShouldReturnDynamicList_WhenSqlQueryIsExecuted()
		//   {
		//       // Arrange
		//       var mockDbContext = new Mock<DbContext>();
		//       var mockDatabaseFacade = new Mock<DatabaseFacade>(mockDbContext.Object);
		//       var mockDbConnection = new Mock<IDbConnection>();
		//       var mockDbCommand = new Mock<IDbCommand>();
		//       var mockDataReader = new Mock<IDataReader>();

		//       string query = "SELECT * FROM Users";
		//       string connectionString = "FakeConnectionString";
		//       var parameters = new Dictionary<string, object>
		//       {
		//           { "@Param1", "Value1" }
		//       };

		//       // Mock the Database property in DbContext
		//       mockDbContext.Setup(x => x.Database).Returns(mockDatabaseFacade.Object);

		//       // Mock the extension method GetDbConnection
		//       mockDatabaseFacade.Setup(x => x.GetDbConnection()).Returns((System.Data.Common.DbConnection)mockDbConnection.Object);

		//	// Mock connection
		//	mockDbConnection.Setup(x => x.CreateCommand()).Returns(mockDbCommand.Object);
		//       mockDbConnection.Setup(x => x.ConnectionString).Returns(connectionString);
		//       mockDbConnection.Setup(x => x.State).Returns(ConnectionState.Closed);
		//       mockDbConnection.Setup(x => x.Open());

		//       // Mock command
		//       mockDbCommand.SetupProperty(x => x.CommandText, query);
		//       mockDbCommand.Setup(x => x.Connection).Returns(mockDbConnection.Object);
		//       mockDbCommand.Setup(x => x.CreateParameter())
		//           .Returns(new Mock<IDbDataParameter>().Object);
		//       mockDbCommand.Setup(x => x.ExecuteReader()).Returns(mockDataReader.Object);

		//       // Mock DataReader
		//       mockDataReader.Setup(x => x.FieldCount).Returns(2);
		//       mockDataReader.SetupSequence(x => x.Read())
		//           .Returns(true) // First row
		//           .Returns(false); // End of rows
		//       mockDataReader.Setup(x => x.GetName(It.Is<int>(i => i == 0))).Returns("Id");
		//       mockDataReader.Setup(x => x.GetName(It.Is<int>(i => i == 1))).Returns("Name");
		//       mockDataReader.Setup(x => x[It.Is<int>(i => i == 0)]).Returns(1);
		//       mockDataReader.Setup(x => x[It.Is<int>(i => i == 1)]).Returns("John Doe");

		//       // Act
		//       var result = mockDbContext.Object.DynamicListFromSql(query, connectionString, parameters);

		//       // Assert
		//       Assert.NotNull(result);
		//       var list = result.ToList();
		//       Assert.Single(list);

		//       var firstRow = list.First();
		//       Assert.Equal(1, firstRow.Id);
		//       Assert.Equal("John Doe", firstRow.Name);

		//       // Verify
		//       mockDbCommand.Verify(x => x.ExecuteReader(), Times.Once);
		//       mockDbConnection.Verify(x => x.Open(), Times.Once);
		//       mockDbConnection.Verify(x => x.Close(), Times.Once);
		//   }
	}

}
