using System;

namespace LibraryAsDll_48
{
    public interface IAsyncShutdown
    {
        /// <summary>
        /// Toto zavolá thread z druhé domény jako úplně poslední akci.
        /// Volá se vždy a pouze jednou.
        /// </summary>
        event EventHandler HasShutdown;

        void BeginShutdown();
    }
}