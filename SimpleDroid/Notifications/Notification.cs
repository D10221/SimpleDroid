using System;
using System.Threading.Tasks;
using Android.App;
using Android.Support.Design.Widget;

namespace SimpleDroid
{
    public interface INotification
    {
        Task<Notification.IResult> Notify(Activity activity);
    }

    public class Notification: INotification
    {
        int Duration  = 0;

        private readonly string _message;

        private readonly string _actionMessage;

        public Notification(string message, string actionMessage)
        {
            _message = message;
            _actionMessage = actionMessage;
        }

        public Task<IResult> Notify(Activity activity)
        {
            var completion = new TaskCompletionSource<IResult>();

            Action notify = () =>
            {
                var foo = Duration > 0 ? Duration : Snackbar.LengthShort;
                using (var makeText = Snackbar.Make(activity.Window.DecorView, _message, foo ))
                {
                    makeText.SetAction(_actionMessage, (v) =>
                    {
                        completion.SetResult(new Result
                        {
                            Ok = true
                        });
                        completion = null;
                    });
                   
                    // Dimissed 
                    makeText.SetCallback(new SnackbarCallback(s =>
                    {   
                        // TODO: Prevent Exception if Activity//Snackbar closing/closed/Disposed/etc...  , return 
                        completion?.SetResult(new Result());
                    }));

                    makeText.Show();
                }
            };
            
            activity.RunOnUiThread(notify);            
            
            return completion.Task;
        }

        public interface IResult
        {
            bool Ok { get; }
        }

        class Result : IResult
        {
            public bool Ok { get; set; }
        }
    }
}