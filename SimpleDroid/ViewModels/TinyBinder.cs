using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Threading;
using Android.Widget;

namespace SimpleDroid
{
    static class TinyBinder
    {
        
        public static IDisposable BindTo<T, TR, TView>(
            this TView view,
            T source,
            Expression<Func<T, TR>> memberExpression,
            Action<T, object> onViewChanged = null ) where T : INotifyPropertyChanged where TView: TextView
        {
            var propertyName = (memberExpression.Body as MemberExpression)?.Member.Name;

            if (string.IsNullOrWhiteSpace(propertyName)) { throw new ArgumentException("Invalild Member Expression", nameof(memberExpression)); }

            var getValue = memberExpression.Compile();

            var subs = source
                .WhenPropertyChanged(propertyName)
                .Select(x => getValue(source))
                .StartWith(getValue(source))
                .DistinctUntilChanged()
                .Where(value => !(view.Text?.Equals(value?.ToString()) ?? false))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(value => { view.Text = value?.ToString(); });

            if (onViewChanged == null) return subs;

            var subs2 = view.
                OnTextChanged()
                .Select(x => (x.Sender as EditText)?.Text)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(text => onViewChanged(source, text));

            return new Disposable(() =>
            {
                subs.Dispose();
                subs2.Dispose();
            });
        }        
    }    
}