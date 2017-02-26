using System;
using System.Reactive.Linq;
using System.Threading;

namespace SimpleDroid
{
    public static class ActivityBaseExtensions
    {
        public static IObservable<IEvent> WhenBackPressed(this ActivityBase activity)
        {
            return activity.Events.Where(e => e.Key == nameof(activity.OnBackPressed));
        }

        /// <summary>
        /// Once
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static IObservable<ActivityState> OnDestroying(this ActivityBase activity)
        {
            return activity
                .Events
                .Where(e => e.Key == nameof(ActivityBase.ActivityState))
                .Select(x => (ActivityState) x.Value)
                .Where(state => state == ActivityState.Destroying)
                .Take(1);
        }       
    }    
}