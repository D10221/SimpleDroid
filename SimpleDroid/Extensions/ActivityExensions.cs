using Android.App;
using Android.Content;

namespace SimpleDroid
{
    public static class ActivityExensions
    {
        public static void StartActivity<T>(this ContextWrapper activity, Context context = null )
        {
            activity.StartActivity(new Intent(context ?? Application.Context, typeof(T)));
        }        
    }
}
