using System;
using Android.Runtime;
using NLog;
using TinyIoC;

namespace SimpleDroid
{
    public abstract class DroidApp: Android.App.Application
    {
        private Logger _logger;

        private NLog.Logger Logger => _logger ?? (_logger = NLog.LogManager.GetCurrentClassLogger());

        public virtual TinyIoCContainer Container { get; } = TinyIoCContainer.Current;

        public DroidApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {            
            Initialize();
        }        
        protected abstract ITinyIocBootstraper Bootstraper { get; }

        protected void Initialize()
        {    
            try
            {                
                Bootstrap();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        protected virtual void Bootstrap()
        {
            Bootstraper.ConfigureApplicationContainer(Container);
        }

        public override void OnCreate()
        {
            try
            {
                Initialize();
                Logger.Info("OnCreated");
                base.OnCreate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }        

        public void Exit()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
