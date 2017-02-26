using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace SimpleDroid
{
    public static class ProperyChangedExtension
    {
        public static IObservable<EventPattern<PropertyChangedEventArgs>> WhenPropertyChanged<T>(this T source,
            string propertyName)
            where T : INotifyPropertyChanged
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            return
                Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        x => source.PropertyChanged += x, x => source.PropertyChanged -= x)
                    .Where(e => e.EventArgs.PropertyName == propertyName);
        }

        public static IObservable<EventPattern<PropertyChangedEventArgs>> WhenPropertyChanged<T>(this T source,
            params string[] propertyNames)
            where T : INotifyPropertyChanged
        {
            if (propertyNames == null || !propertyNames.Any()) throw new ArgumentNullException(nameof(propertyNames));

            return
                Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        x => source.PropertyChanged += x, x => source.PropertyChanged -= x)
                    .Where(e => propertyNames.Contains(e.EventArgs.PropertyName));
        }
    }
}