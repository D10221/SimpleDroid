using System;
using System.Collections.Generic;

namespace SimpleDroid
{
    public static class DisposableExtensions
    {
        public static void ToBeDisposedBy(this IDisposable disposable , ISubscriber subscriber)
        {
            subscriber.Subscriptions.Add(disposable);
        }
        public static void ToBeDisposedBy(this IDisposable disposable , ISubscriber subscriber, string name)
        {
            subscriber.Subscriptions.Add(new Subscription(disposable.Dispose, name));
        }

        public static void Dispose(this IEnumerable<IDisposable> disposables)
        {
            foreach (var disposable in disposables)
            {
                disposable?.Dispose();
            }
        }

        public static void Dispose(this ISubscriber dispopser)
        {
            dispopser?.Subscriptions?.Dispose();
        }
    }
}