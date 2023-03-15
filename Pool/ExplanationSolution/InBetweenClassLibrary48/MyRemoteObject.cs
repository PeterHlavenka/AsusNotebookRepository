using System;

namespace InBetweenClassLibrary48
{
    // Pres referenci marshalovany objekt, ktery zna server i client (maji nareferencovanou dll tohoto projektu)
    public sealed class MyRemoteObject : MarshalByRefObject
    {
        public int Add(int x, int y)
        {
            Console.WriteLine("MyRemoteObject.Add() called");
            
            RaiseChanged();
            return x + y;
        }

        public event Action Changed;

        private void RaiseChanged()
        {
            Changed?.Invoke();
        }
    }
}