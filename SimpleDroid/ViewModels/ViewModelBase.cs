using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;

namespace SimpleDroid
{
    public class ViewModelBase: IActivityViewModel, IDisposer
    {
        private Logger _logger;
        protected Logger Logger => _logger ?? (_logger = LogManager.GetCurrentClassLogger());
        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

        public event PropertyChangedEventHandler PropertyChanged;

        // [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void Dispose()
        {
           Disposables?.Dispose();
        }

        /// <summary>
        /// Called after this ViewModel is Injected/BuildUp by the Injector 
        /// into a view 
        /// </summary>        
        public virtual void OnBuiltUp(object view)
        {            
            View = view;
        }

        private object _view;

        public object View
        {
            get { return _view; }
            set
            {
                if (_view == value) return;
                _view = value;
                RaisePropertyChanged();
                OnViewAttached(View);
            }
        }

        protected virtual void OnViewAttached(object view)
        {

        }
    }
}