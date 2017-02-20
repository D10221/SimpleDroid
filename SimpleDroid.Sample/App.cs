using System;
using Android.App;
using Android.Runtime;
using TinyIoC;

namespace SimpleDroid
{
    [Application]
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class App : DroidApp
    {
        
        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {            
            Initialize();
        }

        protected override void Bootstrap()
        {
            Current = this;
            base.Bootstrap();
        }

        
        public static App Current { get; private set; }
        protected override ITinyIocBootstraper Bootstraper { get; } = new Bootstraper();
    }
}