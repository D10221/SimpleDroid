using System;

namespace SimpleDroid
{
    public interface ISubscribable
    {
        IObservable<IEvent> Events { get; }
    }
}
