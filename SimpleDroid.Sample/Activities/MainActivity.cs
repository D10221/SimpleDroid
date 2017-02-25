using System;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace SimpleDroid
{    
    [Activity(
        Label = "@string/ApplicationName", 
        // MainLauncher = true, 
        Theme = "@style/MyTheme",
        Icon = "@drawable/icon")]
    public class MainActivity : ActivityBase
    {        
        public MainActivity()
        {
            ExitDialog = TinyIoC.TinyIoCContainer.Current.Resolve<IDialog>(nameof(ExitDialog));            
        }

        #region backing fields

        #endregion

        protected override int ToolbarTitle { get; } = Resource.String.ApplicationName;
        public override int FragmentContainerID { get; } = Resource.Id.fragment_container;


        private INotification _pressBackAgainToExitNotification;
        protected override INotification PressBackAgainToExitNotification
        {
            get { return _pressBackAgainToExitNotification ?? (_pressBackAgainToExitNotification = new Notification(
                    GetString(Resource.String.press_back_again_to_exit),
                    GetString(Resource.String.i_got_it))); }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.When(nameof(OnNavigationItemSelected))
                .Select(e => (e.Value as IMenuItem)?.ItemId ?? 0 )
                .Where(itemId => itemId > 0)
                .DistinctUntilChanged()
                .StartWith(Resource.Id.nav_home)
                .Subscribe(Navigator.Navigate)
                .ToBeDisposedBy(this);

            this.When(nameof(OnNavigationItemSelected))
                .Select(e => (e.Value as IMenuItem)?.ItemId ?? 0 )
                .Where(itemId => itemId ==  Resource.Id.nav_exit)                        
                .Subscribe(i=> Exit())
                .ToBeDisposedBy(this);           

        }

        protected override IDialog ExitDialog { get; }
        protected override View View => LayoutInflater.Inflate(Resource.Layout.Main, null);
        protected override Toolbar Toolbar => FindViewById<Toolbar>(Resource.Id.Toolbar);
        protected override DrawerLayout Drawer => FindViewById<DrawerLayout>(Resource.Id.app_bar_main);
        protected override NavigationView NavigationView => FindViewById<NavigationView>(Resource.Id.navigation_view);
    }
}

