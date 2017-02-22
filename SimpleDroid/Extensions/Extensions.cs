using System;
using Android.App;

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

    public static class FragmentManagerExtensions
    {
        public static void ClearBackStack(this FragmentManager manager)
        {
            for (var i = 0; i < manager.BackStackEntryCount; i++)
            {
                manager.PopBackStack();
            }
        }
    }
}
