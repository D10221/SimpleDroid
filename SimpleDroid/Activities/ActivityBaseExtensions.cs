using System;
using System.Reactive.Linq;

namespace SimpleDroid
{
    public static class ActivityBaseExtensions
    {
        public static IObservable<IEvent> WhenBackPressed(this ActivityBase activity)
        {
            return activity.Events.Where(e => e.Key == nameof(activity.OnBackPressed));
        }

        /// <summary>
        /// TODO: -> NotificationManager IsDisable(INotification) 
        /// </summary>        
        public static IDisposable SubscribeNotification(
            this ActivityBase activity,
            IObservable<IEvent> activityEvents,
            INotification notification)
        {
            var waiting = false;
            var notify = true;
            
                return activityEvents
                    .Select(x => waiting)
                    .Where(wait => !wait)
                    .TakeWhile(x => notify)
                    .Subscribe(async x =>
                    {
                        var result = await notification.Notify(activity);
                        notify = !result.Ok;
                        waiting = false;
                    });        
        }
    }
}