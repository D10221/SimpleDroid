using Android.Content;

namespace SimpleDroid
{
    public static class  IntentExtensions
    {
        public static IntentFilter AddActions(this IntentFilter filter, params string[] actions)
        {
            foreach (var action in actions)
            {
                filter.AddAction(action);
            }
            return filter;
        }
    }
}