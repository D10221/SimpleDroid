using System;
using System.Reactive.Linq;
using Android.Views;
using SimpleDroid;

public static class MenuItemExtensions
{
    public static IObservable<IMenuItem> WhenMenuItemClick<T>(this T f) where T : IMenuItemOnMenuItemClickListener, ISubscribable
    {
        return f.When(nameof(f.OnMenuItemClick))
            .Select(e => e.Value as IMenuItem);

    }
}