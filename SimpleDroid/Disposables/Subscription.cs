using System;
using System.Reactive.Disposables;

namespace SimpleDroid
{
    public interface ISubscriber : IDisposable
    {
        CompositeDisposable Subscriptions { get; }
    }
    public class Subscription : IDisposable
    {
        public bool Unsubscribed { get; private set; }
        public string Name { get; }

        private readonly Action _unsubscribe;
        
        public Subscription(Action unsubscribe, string name = null)
        {
            _unsubscribe = unsubscribe;
            Name = name;
        }

        public void Dispose()
        {
            if (Unsubscribed) return;
            Unsubscribed = true;
            _unsubscribe?.Invoke();
        }
    }
}