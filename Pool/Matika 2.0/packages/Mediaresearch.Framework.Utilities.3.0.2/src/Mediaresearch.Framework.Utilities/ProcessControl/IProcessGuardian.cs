using System.Collections.Generic;

namespace Mediaresearch.Framework.Utilities.ProcessControl
{
    public interface IProcessGuardian
    {
        void AddProcess(ProcessWraper processWraper);

        void RemoveProcess(ProcessWraper processWraper);

        void KillAllProcess();

        void KillAllProcessExecutedFromPreviousServiceInstance();

        /// <summary>
        /// Pokusi se najit vsechny procesy v argumentu a zabije je
        /// </summary>
        /// <param name="fullNames">Procesy s plnym nazvem - cesta a nazev spusteneho souboru</param>
        void KillAllProcess(string[] fullNames);
    }
}