using System;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using SimpleDroid.Extensionsss;

namespace SimpleDroid
{    
    [Activity(
        Label = "@string/ApplicationName", 
        // MainLauncher = true, 
        Theme = "@style/MyTheme",
        Icon = "@drawable/icon")]
    public class MainActivity : ActivityBase
    {
        #region backing fields

        #endregion

        protected override int ActivityLayout { get; } = Resource.Layout.Main;
        protected override int ToolbarLayout { get; } = Resource.Id.Toolbar;
        protected override int ToolbarTitle { get; } = Resource.String.ApplicationName;
        protected override int DrawerLayoutID { get; } = Resource.Id.app_bar_main;
        protected override int NavigationViewID { get; } = Resource.Id.navigation_view;
        public override int FragmentContainerID { get; } = Resource.Id.fragment_container;
        protected override int PressBackAgainToExit { get; } = Resource.String.press_back_again_to_exit;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.When(nameof(base.OnNavigationItemSelected))
                .Select(e => (e.Value as IMenuItem)?.ItemId ?? 0 )
                .Where(itemId => itemId > 0)
                .DistinctUntilChanged()
                .StartWith(Resource.Id.nav_home)
                .Subscribe(Navigator.Navigate)
                .ToBeDisposedBy(this);

            this.When(nameof(base.OnNavigationItemSelected))
                .Select(e => (e.Value as IMenuItem)?.ItemId ?? 0 )
                .Where(itemId => itemId ==  Resource.Id.nav_exit)                        
                .Subscribe(i=> Exit())
                .ToBeDisposedBy(this);           

        }

        private IDialog _exitDialog;

        protected override IDialog ExitDialog
        {
            get
            {
                if (_exitDialog != null) return _exitDialog;
                _exitDialog = Container.ResolveMe<IDialog>();
                return _exitDialog;
            }
        }
        

    }    

   
}

