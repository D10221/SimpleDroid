using System;
using System.Reactive.Linq;
using System.Threading;

namespace SimpleDroid
{
    public static class NotificationExtensions
    {
        /// <summary>
        /// TODO: -> NotificationManager IsDisable(INotification) 
        /// </summary>        
        public static IDisposable SubscribeNotification(
            this ActivityBase activity,
            IObservable<IEvent> activityEvents,
            INotification notification)
        {
            var waiting = false;
            var enabled = true;

            var source = new CancellationTokenSource();

            return activityEvents
                .Select(x => new
                {
                    waiting,
                    enabled,
                    // activity.ActivityState,
                    activity.IsDestroyed,
                    source.IsCancellationRequested
                })
                .Where(x => !x.waiting)
                .Do(x =>
                {
                    if (!x.IsCancellationRequested && x.IsDestroyed)
                        source.Cancel();
                })
                .TakeWhile(x =>
                    x.enabled  // it's Disabled ?
                    && !x.IsDestroyed
                    && !x.IsCancellationRequested)

                .Subscribe(async x =>
                {
                    var result = await notification.Notify(activity, source.Token);
                    enabled = !result.Ok;
                    waiting = false;

                    if (source.IsCancellationRequested) return;

                    source.Cancel();
                });
        }
    }
}