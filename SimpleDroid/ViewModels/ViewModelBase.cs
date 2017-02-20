using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;

namespace SimpleDroid
{
    public class ViewModelBase: IViewModel, IDisposer
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
            // ... 
        }        
    }
}