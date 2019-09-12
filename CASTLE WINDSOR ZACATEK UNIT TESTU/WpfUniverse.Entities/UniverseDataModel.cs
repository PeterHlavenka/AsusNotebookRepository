using BLToolkit.Data.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using System.Data;
using BLToolkit.Data.DataProvider;

namespace WpfUniverse.Entities
{
    public class UniverseDataModel : TransactionDbManager
    {
        public UniverseDataModel(DataProviderBase dataProvider, string connectionString) : base(dataProvider, connectionString)
        {
        }

        public UniverseDataModel(DataProviderBase dataProvider, IDbConnection connection) : base(dataProvider, connection)
        {
        }

        public UniverseDataModel(DataProviderBase dataProvider, IDbTransaction transaction) : base(dataProvider, transaction)
        {
        }

        public UniverseDataModel()
        {
        }

        public UniverseDataModel(string configurationString) : base(configurationString)
        {
        }

        public UniverseDataModel(string providerName, string configuration) : base(providerName, configuration)
        {
        }

        public UniverseDataModel(IDbConnection connection) : base(connection)
        {
        }

        public UniverseDataModel(IDbTransaction transaction) : base(transaction)
        {
        }


        public Table<Galaxie> Galaxie => GetTable<Galaxie>();
        public Table<Planet> Planeta => GetTable<Planet>();
        public Table<Vlastnost> Vlastnost => GetTable<Vlastnost>();
        public Table<VlastnostiPlanet> VlastnostiPlanet => GetTable<VlastnostiPlanet>();
    }
}
