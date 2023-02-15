using System;

namespace ServerCore;

public class Communicator
{
    public event EventHandler OnSendMessage;

    public virtual void OnOnSendMessage()
    {
        OnSendMessage?.Invoke(this, EventArgs.Empty);
    }
}