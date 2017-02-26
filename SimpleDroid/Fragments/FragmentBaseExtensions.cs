using System;
using System.Reactive.Linq;
using Android.Views;

namespace SimpleDroid
{
    public static class FragmentBaseExtensions
    {
        /// <summary>
        /// Once
        /// </summary>        
        public static IObservable<View> WhenViewCreated(this FragmentBase fragment)
        {
            return fragment
                .When(nameof(FragmentBase.OnCreateView))                                
                .Select(@event => @event.Value as View)
                .Where(e=> e!=null)
                .Take(1);
        }}
    }