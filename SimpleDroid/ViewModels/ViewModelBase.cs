using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
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
            foreach (var disposable in Disposables)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Called after this ViewModel is Injected/BuildUp by the Injector 
        /// </summary>        
        public virtual void BuildUp(ContextWrapper activity)
        {            
            ViewContext = activity;
        }

        public ContextWrapper ViewContext { get; set; }
    }
}