using System;

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
}
