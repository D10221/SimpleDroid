using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;

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

            view.SetBackgroundResource(Resource.Drawable.splash_screen);

            var layerDrawable = (view.Background as LayerDrawable);
            this._animation = layerDrawable.Drawables().FirstOrDefault(x=>x.GetType() == typeof(AnimationDrawable)) as AnimationDrawable;
            if (_animation == null)
            {
                Log( $"Window.DecorView.RootView.Background as AnimationDrawable is NotFound");
                return;
            }

            foreach (var frame in _animation.Frames())
            {
                var bitmap = frame as BitmapDrawable;
                if (bitmap != null)
                {
                    bitmap.Gravity = GravityFlags.Center;
                }
            }
            
            view.Visibility = ViewStates.Visible;
            

            Log();
        }

        private void Log(string message = null, [CallerMemberName] string callerName = null)
        {
            Android.Util.Log.Debug(Tag, $"{callerName??"?"}: {message}");
        }

        protected override async void OnResume()
        {
            base.OnResume();
            
            _animation?.Start();

            await Task.Delay(2000);             

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));            
        }
    }
}