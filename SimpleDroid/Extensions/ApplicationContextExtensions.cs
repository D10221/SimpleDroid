using Android.Content;

namespace SimpleDroid
{
    public static class ApplicationContextExtensions
    {
        public static T GetSystemService<T>(this Context context, string serviceName) where T :Java.Lang.Object
        {
            return (T) context.GetSystemService(serviceName);
        }
    }
}