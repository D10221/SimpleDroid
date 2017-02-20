using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleDroid
{
    public abstract class EntityBase: INotifyPropertyChanged
    {
        private bool _isDirty;

        [SQLite.Ignore]
        public virtual bool IsDirty
        {
            get { return _isDirty; }
            private set
            {
                if (value == _isDirty) return;
                _isDirty = value;
                RaisePropertyChanged();
            }
        }

        public void SetDirty(bool value, bool silent = true)
        {
            if (silent)
            {
                _isDirty = value;
                return;
            }

            IsDirty = value;
        }


        protected IDictionary<string, object> BackingFields { get; } = new ConcurrentDictionary<string, object>();

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            object value;
            if (!BackingFields.TryGetValue(propertyName, out value))
            {
                return default(TValue);
            }
            return (TValue) value;
        }

        protected void SetValue(object value, [CallerMemberName] string propertyName = null)
        {
            if(string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            object oldValue;

            if (BackingFields.TryGetValue(propertyName, out oldValue) && (oldValue?.Equals(value) ?? false))
            {
                return;
            }

            BackingFields[propertyName] = value;

            IsDirty = true;

            RaisePropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}