using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace SimpleDroid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override async void OnResume()
        {
            base.OnResume();

            await Task.Delay(2000); 

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));            
        }
    }
}