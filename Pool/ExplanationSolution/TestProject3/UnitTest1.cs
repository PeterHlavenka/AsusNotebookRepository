using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using NUnit.Framework;

namespace TestProject3
{
    public class Tests
    {
        private const int InitializedOrderBase = 1000;
        private const string TableName = "#TestDbClassTemp";
        private DataConnection m_dataConnection;
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var strings = new string[] { "McHale", "mchale", "MCHALE", "mChale", "mCHale", "MCHale", "mcHale" };
            var currentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var eq1 = strings.Where(s => s.Equals("mchale", StringComparison.CurrentCultureIgnoreCase)); // dokumnetac
            var expected1 = new string[] { "mchale", "MCHALE", "mChale", "mCHale", "MCHale" };
            Assert.That(eq1.SequenceEqual(expected1));

            var eq2 = strings.Where(s => s.Equals("McHale", StringComparison.CurrentCultureIgnoreCase));
            var expected2 = new string[] { "McHale", "mcHale" };
            Assert.That(eq2.SequenceEqual(expected2));
        }
        
        [Test, Order(InitializedOrderBase + 1)]
        public void CheckWhere_QueryContainsNotEquals_NotContainsISNULL()
        {
            const string ConnectionString =
                "Data Source=Server=sqltest02;Database=Adwind_CZTV_RC;Integrated Security=SSPI";

            using (var db = SqlServerTools.CreateDataConnection(ConnectionString))
            {
                var q =
                    (from c in db.GetTable<Product>()
                    select c).ToList();
            }

            // inicializace 
             // DbToolkit.Initialization.Initialize();
             LinqToDB.Common.Configuration.Linq.CompareNullsAsValues = false; 
             
             var queryable = GetTempTable().Where(c => c.IntColumn != 0);
             var query = (IExpressionQuery) queryable;
             Assert.That(query.SqlText, Does.Not.Contain("IS NULL"));
        }
        
        private ITable<TestDbClass> GetTempTable()
        {
            return m_dataConnection.GetTable<TestDbClass>();
        }
        
        [Table(TableName)]
        private class TestDbClass
        {
            [Column] public int Id { get; set; }
            [Column] public DateTime Date { get; set; }
            [Column] public int? IntColumn { get; set; }
        }
    }
}