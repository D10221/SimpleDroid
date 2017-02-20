using System.Collections.Generic;
using Android.Views;

namespace SimpleDroid
{
    public static class MenuExtensions
    {
        public static IEnumerable<IMenuItem> Items(this IMenu menu)
        {            
            for (var i = 0; i < menu.Size(); i++)
            {
                yield return menu.GetItem(i);
            }
        }        
    }
}