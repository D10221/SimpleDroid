using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace SimpleDroid
{
    public abstract class EventsBase: IHaveEvents
    {
        protected readonly Subject<IEventArgs> _events = new Subject<IEventArgs>();
        public IObservable<IEventArgs> Events => _events.AsObservable();

        protected virtual void RaiseEvent(object value = null, [CallerMemberName] string callerName = null)
        {
            _events.OnNext(new ActivityEventArgs(callerName, value));
        }
    }
}