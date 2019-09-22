using System.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using Mediaresearch.Framework.DataAccess.BLToolkit;

namespace Entities
{
    class EnumeratedWordsDataModel : TransactionDbManager
    {
        public EnumeratedWordsDataModel(DataProviderBase dataProvider, string connectionString)
            : base(dataProvider, connectionString)
        {
        }

        public EnumeratedWordsDataModel(DataProviderBase dataProvider, IDbConnection connection)
            : base(dataProvider, connection)
        {
        }

        public EnumeratedWordsDataModel(DataProviderBase dataProvider, IDbTransaction transaction)
            : base(dataProvider, transaction)
        {
        }

        public EnumeratedWordsDataModel()
        {
        }

        public EnumeratedWordsDataModel(string configurationString)
            : base(configurationString)
        {
        }

        public EnumeratedWordsDataModel(string providerName, string configuration)
            : base(providerName, configuration)
        {
        }

        public EnumeratedWordsDataModel(IDbConnection connection)
            : base(connection)
        {
        }

        public EnumeratedWordsDataModel(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public Table<BWord> Galaxie => GetTable<BWord>();
    }
}
