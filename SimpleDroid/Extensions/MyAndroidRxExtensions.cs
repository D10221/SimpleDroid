using System;
using System.Reactive;
using System.Reactive.Linq;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace SimpleDroid
{
    public static class MyAndroidRxExtensions
    {
        public static IObservable<EventPattern<EventArgs>> OnClick(this View source)
        {
            return Observable.FromEventPattern<EventHandler, EventArgs>(x => source.Click += x, x => source.Click -= x);
        }
        public static IObservable<EventPattern<TextChangedEventArgs>> OnTextChanged(this Button b)
        {
            return Observable.FromEventPattern<EventHandler<TextChangedEventArgs>, TextChangedEventArgs>(
                x => b.TextChanged += x, x => b.TextChanged -= x
            );
        }

        public static IObservable<EventPattern<TextChangedEventArgs>> OnTextChanged(this TextView b)
        {
            return Observable.FromEventPattern<EventHandler<TextChangedEventArgs>, TextChangedEventArgs>(
                x => b.TextChanged += x, x => b.TextChanged -= x
            );
        }

        public static IObservable<EventPattern<NavigationView.NavigationItemSelectedEventArgs>> OnNavigationItemSelected(this NavigationView view)
        {
            return Observable.FromEventPattern<EventHandler<NavigationView.NavigationItemSelectedEventArgs>, NavigationView.NavigationItemSelectedEventArgs>(
                x => view.NavigationItemSelected += x, x => view.NavigationItemSelected -= x
            );
        }

        public static IObservable<EventPattern<DrawerLayout.DrawerOpenedEventArgs>> OnDrawerOpened(this DrawerLayout drawer)
        {
            drawer.DrawerOpened += OnOpen;
            return
                Observable
                    .FromEventPattern
                    <EventHandler<DrawerLayout.DrawerOpenedEventArgs>, DrawerLayout.DrawerOpenedEventArgs>(
                        x => drawer.DrawerOpened += x, x => drawer.DrawerOpened -= x);
        }

        private static void OnOpen(object sender, DrawerLayout.DrawerOpenedEventArgs e)
        {
            

        }
    }
}
