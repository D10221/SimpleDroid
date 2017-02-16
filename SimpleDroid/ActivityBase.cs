using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using NLog;
using TinyIoC;

namespace SimpleDroid
{
    public abstract class ActivityBase : AppCompatActivity, IHaveDisposables, View.IOnClickListener
    {
        private Logger _logger;
        protected TinyIoCContainer Container { get; } = TinyIoCContainer.Current;

        private NavigationView _navigationView;
        
        protected ActivityBase()
        {
            Initialize();
        }

        protected ActivityBase(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        protected Logger Logger => _logger ?? (_logger = LogManager.GetCurrentClassLogger());
        protected abstract int ViewName { get; }
        protected abstract int ActivityLayout { get; }

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


        protected virtual NavigationView NavigationView
        {
            get
            {
                if (_navigationView != null) return _navigationView;
                _navigationView = FindViewById<NavigationView>(NavigationViewID);
                return _navigationView;
            }
        }

        protected virtual int OpenDrawerContentDescRes { get; } = 0;
        protected virtual int CloseDrawerContentDescRes { get; } = 0;
        protected virtual int ToolbarMenuLayout { get; private set; } = 0;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(ActivityLayout);

            if (Toolbar == null) return;

            SetSupportActionBar(Toolbar);
            SupportActionBar.SetTitle(ViewName);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            NavigationView?.OnNavigationItemSelected()
                .Subscribe(e => OnNavigationItemSelected(e.Sender, e.EventArgs))
                .ToBeDisposedBy(this);

            if (Drawer == null) return;

            var drawerToggle = new ActionBarDrawerToggle(
                /*activity:*/ this,
                /*drawerLayout:*/ Drawer, 
                /*toolbar: */Toolbar,
                OpenDrawerContentDescRes,
                CloseDrawerContentDescRes);

            Drawer.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            drawerToggle.ToBeDisposedBy(this);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            InflateMenu(menu, ToolbarMenuLayout);
            return base.OnCreateOptionsMenu(menu);
        }

        private void InflateMenu(IMenu menu, int toolbarMenuLayout)
        {
            if (toolbarMenuLayout > 0)
            {
                if (menu != null)
                {
                    menu.Clear();

                    MenuInflater.Inflate(toolbarMenuLayout, menu);

                    OnMenuInflated(menu);
                }
            }
        }

        protected virtual void OnMenuInflated(IMenu menu)
        {
            RaiseEvent(menu);
            //menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
            //menu.FindItem(Resource.Id.action_attach).SetVisible(false);
        }

        //define action for tolbar icon press
        // public override bool OnOptionsItemSelected(IMenuItem item);


        protected virtual int FragmentContainerID { get; } = 0;


        protected virtual void Navigate(IMenuItem menuItem)
        {
            Navigate(menuItem.ItemId);
        }

        protected virtual void Navigate(int navId)
        {
            if (FragmentContainerID > 0 && navId > 0)
            {
                using (var ft = FragmentManager.BeginTransaction())
                {
                    var fragment = FragmentFactory.Resolve(navId);
                    // (fragment as FragmentBase)?.InflateMenu(_menu, MenuInflater);
                    ft.Add(FragmentContainerID, fragment);
                    ft.Commit();
                }
            }
        }

        public ViewState ViewState { get; private set; }

        public Subject<ActivityEventArgs> ActivityEvents { get; } = new Subject<ActivityEventArgs>();

        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

        private IFragmentFactory _fragmentFactory;

        private IFragmentFactory FragmentFactory
            => _fragmentFactory ?? (_fragmentFactory = Container.Resolve<IFragmentFactory>());

        public virtual void OnClick(View v)
        {
            RaiseEvent(v);
        }

        protected virtual void OnNavigationItemSelected(object sender,
            NavigationView.NavigationItemSelectedEventArgs args)
        {
            ActivityEvents.OnNext(new ActivityEventArgs(sender, nameof(OnNavigationItemSelected), args.MenuItem));
            Navigate(args.MenuItem);
            Drawer.CloseDrawers();
        }


        private void Initialize()
        {
            try
            {
                foreach (var property in PropertyInfos)
                    Inject(property.GetCustomAttribute<InjectAttribute>(), property);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private PropertyInfo[] _propertyInfos;

        private PropertyInfo[] PropertyInfos
            =>
                _propertyInfos ??
                (_propertyInfos =
                    GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));

        private void Inject(InjectAttribute inject, PropertyInfo property)
        {
            if (inject == null) return;

            if (!string.IsNullOrWhiteSpace(inject.Name) || inject.IsNamed)
            {
                try
                {
                    property.SetValue(this,
                        OnInject(Container.Resolve(
                            inject.Type ?? property.PropertyType,
                            inject.Name ?? property.Name
                        )));
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
                return;
            }

            property.SetValue(this, OnInject(
                Container.Resolve(inject.Type ?? property.PropertyType))
            );
        }

        private object OnInject(object resolve)
        {
            (resolve as ViewModelBase)?.OnInject(this);
            return resolve;
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

        protected override void OnDestroy()
        {
            ViewState = ViewState.Destroying;
            RaiseEvent();
            Disposables.Dispose();
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            ViewState = ViewState.Pausing;
            RaiseEvent();
            Disposables.Dispose();
            base.OnPause();
        }

        protected override void OnStop()
        {
            ViewState = ViewState.Stopping;
            RaiseEvent();
            Disposables.Dispose();
            base.OnStop();
        }

        private void RaiseEvent(object value = null, [CallerMemberName] string callerName = null)
        {
            ActivityEvents.OnNext(new ActivityEventArgs(this, callerName, value));
        }

        //to avoid direct app exit on backpreesed and to show fragment from stack
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
            {
                FragmentManager.PopBackStack(); // fragmentManager.popBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}