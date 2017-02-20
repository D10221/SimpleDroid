using System;
using Android.Content;
using Android.Widget;

namespace SimpleDroid
{
    public static class Extensions
    {
        public static T With<T>(this T source, Action<T> action)
        {
            action.Invoke(source);
            return source;
        }
    }

    public static class TextViewExtension
    {
        public static void AddText(this TextView textView, string text)
        {
            textView.Text += text;
        }
    }

    public static class ApplicationContextExtensions
    {
        public static T GetSystemService<T>(this Context context, string serviceName) where T :Java.Lang.Object
        {
            return (T) context.GetSystemService(serviceName);
        }
    }
}
