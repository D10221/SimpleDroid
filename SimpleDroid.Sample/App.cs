using System;
using Android.App;
using Android.Runtime;
using NLog;
using TinyIoC;

namespace SimpleDroid
{
    [Application]
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class App : Application
    {

        private Logger _logger;

        private NLog.Logger Logger => _logger ?? (_logger = NLog.LogManager.GetCurrentClassLogger());

        public TinyIoCContainer Container { get; private set; }


        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Instance = this;
            Initialize();
        }

        private bool _initialized;
        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                Container = TinyIoCContainer.Current;
                Bootstraper.Default.ConfigureApplicationContainer(Container);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

        public static App Instance { get; private set; }
    }
}