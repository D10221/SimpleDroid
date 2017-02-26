using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace SimpleDroid
{
    public abstract class EventsBase: ISubscribable
    {
        private readonly Subject<IEvent> _events = new Subject<IEvent>();
        public IObservable<IEvent> Events => _events.AsObservable();

        protected virtual void RaiseEvent(object value = null, [CallerMemberName] string callerName = null)
        {
            _events.OnNext(new Event(callerName, value));
        }
        private class Event : IEvent
        {
            public Event(string key, object value = null)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; }
            public object Value { get; }
        }
    }    
}