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

        public static IDisposable OnDoubleEvent(this IObservable<IEvent> events,
          Action action,
          int doubleBackPressedWaitingWindow = 2000)
        {
            return events
                //.Timestamp()
                //.Do(x => Logger.Debug($"Back Pressed: {x}"))                
                .Subscribe(x =>
                {
                    events
                        //.Timestamp()
                        .Take(1)
                        .Timeout(TimeSpan.FromMilliseconds(doubleBackPressedWaitingWindow))
                        .Subscribe(e => action.Invoke(), error =>
                        {
                            // ... ignore , timeOut
                        });
                });
        }

    }
}