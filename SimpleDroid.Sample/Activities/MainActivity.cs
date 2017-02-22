using System;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using TinyIoC;

namespace SimpleDroid
{    
    [Activity(
        Label = "@string/ApplicationName", 
        // MainLauncher = true, 
        Theme = "@style/MyTheme",
        Icon = "@drawable/icon")]
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

            this.When(nameof(base.OnNavigationItemSelected))
                .Select(e => (e.Value as IMenuItem)?.ItemId ?? 0 )
                .Where(itemId => itemId ==  Resource.Id.nav_exit)                        
                .Subscribe(Exit)
                .ToBeDisposedBy(this);
        }

        protected virtual DialogFty DialogFty { get; private set; }
        async void Exit(int itemId)
        {
            if (itemId != Resource.Id.nav_exit) return;
            
            DialogFty = new DialogFty(
                Resource.Layout.exit_prompt_dialog,
                Resource.String.yes, 
                Resource.String.no,                 
                Resource.Id.exit_prompt_dontaskagain
                );

            var result = await DialogFty.Show(this, this.LayoutInflater);

            if (result[Resource.Id.exit_prompt_dontaskagain] as bool? ?? false)
            {
                Logger.Info("DontAskAgin");
            }
            if (result[Resource.String.yes] as bool? ?? false)
            {
                Logger.Info("Ok");
            }
        }
        
        void Navigate(int itemId)
        {
            if (itemId == Resource.Id.nav_exit) return;
            Navigator.Navigate(itemId);
        }
    }
}

