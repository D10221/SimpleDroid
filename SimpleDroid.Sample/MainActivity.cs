using System;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using TinyIoC;

namespace SimpleDroid
{    
    [Activity(Label = "SimpleDroid", MainLauncher = true, Theme = "@style/MyTheme", Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivityBase
    {
        #region backing fields

        #endregion

        protected override int ActivityLayout { get; } = Resource.Layout.Main;
        protected override int ToolbarLayout { get; } = Resource.Id.Toolbar;

        protected override int ToolbarTitle { get; } = Resource.String.ApplicationName;
        protected override int DrawerLayoutID { get; } = Resource.Id.app_bar_main;
        protected override int NavigationViewID { get; } = Resource.Id.navigation_view;
        public override int FragmentContainerID { get; } = Resource.Id.fragment_container;

        [Inject]
        private NavigatorFty NavigatorFty { get; set; }

        private Navigator _navigator;

        private Navigator Navigator
        {
            get
            {
                if (_navigator != null) return _navigator;
                _navigator = NavigatorFty.Create(this);
                Container.Register((s, e) => _navigator);
                return _navigator;
            }            
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.When(nameof(base.OnNavigationItemSelected))
                .Select(e => (e.Value as IMenuItem)?.ItemId ?? 0 )
                .Where(itemId => itemId > 0)
                .DistinctUntilChanged()
                .StartWith(Resource.Id.nav_home)
                .Subscribe(Navigate)
                .ToBeDisposedBy(this);
        }

        void Navigate(int itemId)
        {
            if (itemId == Resource.Id.nav_exit)
            {
                App.Current.Exit();
                return;
            }

            Navigator.Navigate(itemId);
        }
    }
}

