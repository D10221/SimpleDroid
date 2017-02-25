using System;
using System.Reactive.Linq;

namespace SimpleDroid
{
    public static class EventsExtensions
    {
        public static IObservable<IEvent> When(this ISubscribable source, string key)
        {
            return source.Events.Where(e => e.Key == key);
        }
    }
}