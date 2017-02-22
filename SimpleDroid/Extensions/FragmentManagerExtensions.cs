using Android.App;

namespace SimpleDroid
{
    public static class FragmentManagerExtensions
    {
        public static void ClearBackStack(this FragmentManager manager)
        {
            for (var i = 0; i < manager.BackStackEntryCount; i++)
            {
                manager.PopBackStack();
            }
        }
        public static void ClearBackStack(this Android.Support.V4.App.FragmentManager manager)
        {
            for (var i = 0; i < manager.BackStackEntryCount; i++)
            {
                manager.PopBackStack();
            }
        }
    }
}