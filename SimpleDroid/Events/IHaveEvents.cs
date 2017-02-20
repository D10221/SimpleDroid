using System;

namespace SimpleDroid
{
    public interface IHaveEvents
    {
        IObservable<IEventArgs> Events { get; }
    }
}
