using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using NLog;
using TinyIoC;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace SimpleDroid
{
    public abstract class ActivityBase : AppCompatActivity, IDisposer, View.IOnClickListener, IHaveEvents
    {
        protected TinyIoCContainer Container { get; } = TinyIoCContainer.Current;

        private Injector _injector;
        Injector Injector => _injector ?? (_injector = new Injector(Container, this));

        private ILogger _logger;
        protected ILogger Logger => _logger ?? (_logger = LogManager.GetCurrentClassLogger());

        protected virtual int ToolbarLayout { get; } = 0;

        private Toolbar _toolbar;

        protected virtual Toolbar Toolbar
        {
            get
            {
                if (_toolbar != null) return _toolbar;
                if (ToolbarLayout > 0)
                    _toolbar = FindViewById<Toolbar>(ToolbarLayout);
                return _toolbar;
            }
        }

        protected virtual int DrawerLayoutID { get; } = 0;

        private DrawerLayout _drawer;

        protected virtual DrawerLayout Drawer
        {
            get
            {
                if (_drawer != null) return _drawer;
                if (DrawerLayoutID > 0)
                    _drawer = FindViewById<DrawerLayout>(DrawerLayoutID);
                return _drawer;
            }
        }

        protected virtual int NavigationViewID { get; } = 0;

        private NavigationView _navigationView;

        protected virtual NavigationView NavigationView
        {
            get
            {
                if (_navigationView != null) return _navigationView;
                _navigationView = FindViewById<NavigationView>(NavigationViewID);
                return _navigationView;
            }
        }

        protected abstract int ToolbarTitle { get; }
        protected abstract int ActivityLayout { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(ActivityLayout);
            Injector.BuildUp();

            CurrentFragmentManager.ClearBackStack();

            if (ExitDialog != null)
            {
                ExitOnDoubleBackPressed().ToBeDisposedBy(this);
            }

            SetupToolBar(Toolbar);

            NavigationView?.OnNavigationItemSelected()
                .Subscribe(e => OnNavigationItemSelected(e.EventArgs))
                .ToBeDisposedBy(this);

            DrawerSetup();
        }

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

            if (ToolbarTitle != 0)
            {
                SupportActionBar.SetTitle(ToolbarTitle);
            }

            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
        }


        protected virtual int OpenDrawerContentDescRes { get; } = 0;
        protected virtual int CloseDrawerContentDescRes { get; } = 0;

        private void DrawerSetup()
        {
            if (Drawer == null) return;

            var drawerToggle = new ActionBarDrawerToggle(
                /*activity:*/ this,
                /*drawerLayout:*/ Drawer,
                /*toolbar: */Toolbar,
                OpenDrawerContentDescRes,
                CloseDrawerContentDescRes);

            Drawer.AddDrawerListener(drawerToggle);
            Drawer.OnDrawerOpened().Subscribe(OnDrawerOpened).ToBeDisposedBy(this);

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

        protected IMenu Menu { get; private set; }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            InflateMenu(menu, ToolbarMenuLayout);
            Menu = menu;
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

        private readonly Subject<IEventArgs> _events = new Subject<IEventArgs>();
        
        public IObservable<IEventArgs> Events => _events.AsObservable();

        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

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
            Disposables.Dispose();
            base.OnDestroy();
        }

        private void RaiseEvent(object value = null, [CallerMemberName] string callerName = null)
        {
            _events.OnNext(new ActivityEventArgs(callerName, value));
        }
        /// <summary>
        /// Text from String's
        /// </summary>
        protected virtual int PressBackAgainToExit { get; } = 0;

        protected virtual int DoubleBackPressedWaitingWindow { get; } = 2000;

        /// <summary>
        /// If Selected NO && DontAskAgain with the DIalog we Can't exit with the back button 
        /// Then We can wait for Double Tap to exit         
        /// </summary>
        /// <returns></returns>
        protected virtual IDisposable ExitOnDoubleBackPressed()
        {
            var waiting = false;

            var backPressed = Events
                .Where(e => e.Key == nameof(OnBackPressed))
                .Where(x => CurrentFragmentManager.IsBackStackEmpty(0)
                && ExitDialog == null 
                || ExitDialog.IsDeadLock())
                .Select(x => waiting)
                .Timestamp();

            // TODO: Setup from settings 
            var notify = true;
            return backPressed
                .Do(x => Logger.Debug($"Back Pressed: {x}"))
                .Where(x => !x.Value)
                .Subscribe( x =>
                {
                    waiting = true;
                    Logger.Debug($"waiting: {waiting}");

                    var exit = backPressed
                        .Take(1)
                        .Timeout(TimeSpan.FromMilliseconds(DoubleBackPressedWaitingWindow))
                        .Subscribe(e =>
                            RunOnUiThread(() => {
                                Logger.Debug($"Exiting");
                                OnBackPressed(exit: true);
                            }),
                            error => Logger.Debug(error.Message)
                        );

                    if (notify)
                    {
                        RunOnUiThread(() =>
                        {
                            var makeText = Snackbar.Make(
                                Window.DecorView.RootView,
                                PressBackAgainToExit,
                                Snackbar.LengthShort
                            );

                            makeText.SetAction("I got it", view =>
                            {
                                notify = false;
                            });

                            makeText.SetCallback(new SnackbarCallback(
                                onDimissedd: bar =>
                                {
                                    exit.Dispose();
                                    Logger.Debug("Dimissed");
                                    waiting = false;
                                    Logger.Debug($"waiting: {waiting}");
                                }
                            ));

                            makeText.Show();
                        });
                    }                                        
                });
        }
        
        protected virtual IDialog ExitDialog { get; } = null;

        protected virtual async void Exit()
        {
            if (ExitDialog == null)
            {
                OnBackPressed(true);
                return;
            }

            var result = await ExitDialog.Show(this, !ExitDialog.LastResult.Ok );

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