using Android.App;
using Android.OS;

namespace SimpleDroid
{    
    [Activity(Label = "SimpleDroid", MainLauncher = true, Theme = "@style/MyTheme", Icon = "@drawable/icon")]
    public class MainActivity : ActivityBase
    {
        #region backing fields

        #endregion

        protected override int ViewName { get; } = Resource.String.ApplicationName;
        protected override int ActivityLayout { get; } = Resource.Layout.Main;
        protected override int ToolbarLayout { get; } = Resource.Id.Toolbar;
        protected override int DrawerLayoutID { get; } = Resource.Id.app_bar_main;
        protected override int NavigationViewID { get; } = Resource.Id.navigation_view;
        protected override int FragmentContainerID { get; } = Resource.Id.fragment_container;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Navigate(Resource.Id.nav_home);
        }
    }
}

