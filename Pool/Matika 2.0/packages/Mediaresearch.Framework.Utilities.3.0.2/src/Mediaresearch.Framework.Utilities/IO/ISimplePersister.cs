using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediaresearch.Framework.Utilities.IO
{
    public interface ISimplePersister<TPersistType>
    {
        Task PersistAsync(string identificator, TPersistType objectToStore);

        void Persist(string identificator, TPersistType objectToStore);

        bool FreeResources(string identificator);

        bool TryGetObjectFor(string identificator, out TPersistType obj);

        TPersistType GetObjectFor(string identificator);

        Dictionary<string, TPersistType> LoadPersistedObjects();
    }
}