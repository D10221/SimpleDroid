using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;

namespace SimpleDroid
{
    [Activity(
        Theme = "@style/MyTheme.Splash",        
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : Activity
    {
        private AnimationDrawable _animation;
        private string Tag { get; } = nameof(SplashActivity);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var view = Window.DecorView.RootView;
            if (view == null)
            {
                Log("RootView not Found");
                return;
            }

            _animation = (view.Background as LayerDrawable)?
                .Drawables()
                .OfType<AnimationDrawable>()
                .FirstOrDefault();

            if (_animation == null)
            {
                Log( $"Window.DecorView.RootView.Background as AnimationDrawable is NotFound");
                return;
            }

            Log();
        }

        private void Log(string message = null, [CallerMemberName] string callerName = null)
        {
            Android.Util.Log.Debug(Tag, $"{callerName??"?"}: {message}");
        }

        protected override async void OnResume()
        {
            _animation?.Start();
            base.OnResume();
            await Task.Delay(2000);             
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));            
        }
    }
}