using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using NLog;
using TinyIoC;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace SimpleDroid
{
    public abstract class ActivityBase : AppCompatActivity, ISubscriber, View.IOnClickListener, ISubscribable
    {
        private TinyIoCContainer Container { get; } = TinyIoCContainer.Current;

        private Injector _injector;
        Injector Injector => _injector ?? (_injector = new Injector(Container, this));

        private ILogger _logger;
        protected ILogger Logger => _logger ?? (_logger = LogManager.GetLogger(GetType().Name));

        protected abstract View View { get; }
        protected abstract Toolbar Toolbar { get; }
        protected abstract DrawerLayout Drawer { get; }
        protected abstract NavigationView NavigationView { get; }

        protected abstract int ToolbarTitle { get; }

        // protected abstract int ActivityLayout { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(View);
            Injector.BuildUp();

            CurrentFragmentManager.ClearBackStack();

            ConfigureExitDialog();

            SetupToolBar(Toolbar);

            NavigationView?.OnNavigationItemSelected()
                .Subscribe(e => OnNavigationItemSelected(e.EventArgs))
                .ToBeDisposedBy(this);

            DrawerSetup(Drawer);
        }

        protected virtual void ConfigureExitDialog()
        {
            if (ExitDialog != null)
            {
                var events = this.WhenBackPressed().Where(x => IsDeadLocked(ExitDialog));

                OnDoubleEvent(
                    events: events,
                    action: () => OnBackPressed(exit: true)
                ).ToBeDisposedBy(this);

                if(PressBackAgainToExitNotification!=null)
                this.SubscribeNotification(
                    events, PressBackAgainToExitNotification                    
                ).ToBeDisposedBy(this);
            }
            else
            {

                OnDoubleEvent(
                    events: this.WhenBackPressed(),
                    action: () => OnBackPressed(exit: true)
                ).ToBeDisposedBy(this);

                if (PressBackAgainToExitNotification != null)
                    this.SubscribeNotification(this.WhenBackPressed(),
                    PressBackAgainToExitNotification
                ).ToBeDisposedBy(this);
            }
        }

        protected abstract INotification PressBackAgainToExitNotification { get; } 

        private INavigatorFty _navigatorFty;
        private INavigatorFty NavigatorFty => _navigatorFty ?? (_navigatorFty = Container.Resolve<INavigatorFty>());

        private INavigator _navigator;

        protected virtual INavigator Navigator
        {
            get
            {
                if (_navigator != null) return _navigator;
                _navigator = NavigatorFty.Create(this);
                Container.Register((s, e) => _navigator);
                return _navigator;
            }
        }

        protected virtual void OnNavigationItemSelected(
            NavigationView.NavigationItemSelectedEventArgs args)
        {
            RaiseEvent(args.MenuItem);
            Drawer.CloseDrawers();
        }

        protected virtual void SetupToolBar(Toolbar toolbar)
        {
            if (toolbar == null) return;

            SetSupportActionBar(Toolbar);

            if (ToolbarTitle != 0) SupportActionBar.SetTitle(ToolbarTitle);

            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
        }


        protected virtual int OpenDrawerContentDescRes { get; } = 0;
        protected virtual int CloseDrawerContentDescRes { get; } = 0;

        protected virtual void DrawerSetup(DrawerLayout drawer)
        {
            if (drawer == null) return;

            var drawerToggle = new ActionBarDrawerToggle(
                /*activity:*/ this,
                /*drawerLayout:*/ drawer,
                /*toolbar: */Toolbar,
                OpenDrawerContentDescRes,
                CloseDrawerContentDescRes);

            drawer.AddDrawerListener(drawerToggle);
            drawer.OnDrawerOpened().Subscribe(OnDrawerOpened).ToBeDisposedBy(this);

            drawerToggle.SyncState();
            drawerToggle.ToBeDisposedBy(this);
        }

        private void OnDrawerOpened(EventPattern<DrawerLayout.DrawerOpenedEventArgs> e)
        {
            var drawerLayout = (e.Sender as DrawerLayout);
            drawerLayout?.BringToFront();
            RaiseEvent(drawerLayout);
        }

        protected virtual int ToolbarMenuLayout { get; private set; } = 0;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            InflateMenu(menu, ToolbarMenuLayout);
            return base.OnCreateOptionsMenu(menu);
        }

        protected virtual void InflateMenu(IMenu menu, int toolbarMenuLayout)
        {
            if (toolbarMenuLayout <= 0 || menu == null) return;

            menu.Clear();

            MenuInflater.Inflate(toolbarMenuLayout, menu);

            OnMenuInflated(menu);
        }

        protected virtual void OnMenuInflated(IMenu menu)
        {
            RaiseEvent(menu);
        }

        public virtual int FragmentContainerID { get; } = 0;
        public ViewState ViewState { get; private set; }

        private readonly Subject<IEvent> _events = new Subject<IEvent>();

        #region ISubscribable

        public IObservable<IEvent> Events => _events.AsObservable();

        protected virtual void RaiseEvent(object value = null, [CallerMemberName] string callerName = null)
        {
            _events.OnNext(new ActivityEvent(callerName, value));
        }

        #endregion

        #region ISubscriptor

        public CompositeDisposable Subscriptions { get; } = new CompositeDisposable();

        #endregion

        public virtual void OnClick(View v)
        {
            RaiseEvent(v);
        }

        protected override void OnRestart()
        {
            ViewState = ViewState.Restarting;
            RaiseEvent();
            base.OnRestart();
        }

        protected override void OnResume()
        {
            ViewState = ViewState.Resuming;
            RaiseEvent();
            base.OnResume();
        }

        protected override void OnStart()
        {
            ViewState = ViewState.Starting;
            RaiseEvent();
            base.OnStart();
        }

        protected override void OnPause()
        {
            ViewState = ViewState.Pausing;
            RaiseEvent();
            base.OnPause();
        }

        protected override void OnStop()
        {
            ViewState = ViewState.Stopping;
            RaiseEvent();
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            ViewState = ViewState.Destroying;
            RaiseEvent();
            Subscriptions.Dispose();
            base.OnDestroy();
        }

        
        protected virtual IDisposable OnDoubleEvent(IObservable<IEvent> events, 
            Action action, 
            int DoubleBackPressedWaitingWindow = 2000)
        {            
            return events                
                //.Timestamp()
                //.Do(x => Logger.Debug($"Back Pressed: {x}"))                
                .Subscribe(x =>
                {                    
                    events
                        //.Timestamp()
                        .Take(1)
                        .Timeout(TimeSpan.FromMilliseconds(DoubleBackPressedWaitingWindow))
                        .Subscribe(e => RunOnUiThread(action), error => Logger.Debug(error.Message));
                });                       
        }
   
        
        protected virtual IDialog ExitDialog { get; } = null;
        
        protected virtual bool IsDeadLocked(IDialog dialog)
        {
            return CurrentFragmentManager.IsBackStackEmpty(0)
                            && dialog == null
                            || dialog.IsDeadLocked();
        }

        protected virtual async void Exit()
        {
            if (ExitDialog == null)
            {
                OnBackPressed(true);
                return;
            }

            var result = await ExitDialog.Show(this, !ExitDialog.LastResult.Ok);

            // Save it somewhere 
            Logger.Info($"DontAskAgain: {result.DontAskAgain}");

            if (result.Ok)
            {
                OnBackPressed(true);
            }
        }

        /// <summary>
        /// Don't exit on OnBackPressed
        /// </summary>
        public override void OnBackPressed()
        {
            RaiseEvent();
            OnBackPressed(false);
        }

        public FragmentManager CurrentFragmentManager => SupportFragmentManager;

        protected virtual void OnBackPressed(bool exit)
        {
            if (exit)
            {
                CurrentFragmentManager.ClearBackStack();
                base.OnBackPressed();
                return;
            }

            if (!CurrentFragmentManager.IsBackStackEmpty())
            {
                CurrentFragmentManager.PopBackStack();
                return;
            }

            if (ExitDialog == null)
            {
                base.OnBackPressed();
                return;
            }

            Action maybeExit = async () =>
            {
                var result = await ExitDialog.Show(this);
                if (result.Ok)
                {
                    CurrentFragmentManager.ClearBackStack();
                    base.OnBackPressed();
                }
            };

            maybeExit();
        }
    }
}