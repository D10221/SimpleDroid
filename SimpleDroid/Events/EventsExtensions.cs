using System;
using System.Reactive.Linq;

namespace SimpleDroid
{
    public static class EventsExtensions
    {
        public static IObservable<IEventArgs> When(this IHaveEvents source, string key)
        {
            return source.Events.Where(e => e.Key == key);
        }
    }
}