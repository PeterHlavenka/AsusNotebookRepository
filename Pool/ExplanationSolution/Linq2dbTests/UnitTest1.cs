
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using NUnit.Framework;

namespace Linq2dbTests;

public class Tests
{
    private const string TableName = "#TestDbClassTemp";
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CheckWhere_QueryContainsNotEquals_NotContainsISNULL()
    {
        //inicializace 
        // DbToolkit.Initialization.Initialize();
            
        var queryable = GetTempTable().Where(c => c.IntColumn != 0);
        var query = (IExpressionQuery) queryable;
        Assert.That(query.SqlText, Does.Not.Contain("IS NULL"));
    }
    
    private ITable<TestDbClass> GetTempTable()
    {
        string connectionString = @"Data Source=sqltest02\Adwind_CZTV_RC;Integrated Security=True;";

        var connection = new DataConnection(SqlServerTools.GetDataProvider(SqlServerVersion.v2016, SqlServerProvider.MicrosoftDataSqlClient), connectionString);
        connection.CreateTempTableAsync<TestDbClass>("TableName");
        
        return connection.GetTable<TestDbClass>();
    }
    
    [Table(TableName)]
    private class TestDbClass
    {
        [Column] public int Id { get; set; }
        [Column] public DateTime Date { get; set; }
        [Column] public int? IntColumn { get; set; }
    }
}