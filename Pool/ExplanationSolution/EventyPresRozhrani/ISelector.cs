using System;

namespace EventyPresRozhrani
{
    public interface ISelector
    {
        // co musi implementovat posilaci trida
        event EventHandler MyEvent;

        void OnMainChanged();
    }
}