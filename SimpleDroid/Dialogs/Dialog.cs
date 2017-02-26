using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace SimpleDroid
{
    public interface IDialogResult
    {
        bool Ok { get; }
        bool DontAskAgain { get; }
    }

    public interface IDialog: ISubscribable
    {
        Task<IDialogResult> Show(Activity activity, bool ignoreLastResult = false);
        IDialogResult LastResult { get; }
    }

    public class DialogClickEventArgsExtended : DialogClickEventArgs
    {
        public bool Ok { get; }
        public DialogClickEventArgsExtended(DialogClickEventArgs args, bool ok) : base(args.Which)
        {
            Ok = ok;
        }
    }
    public abstract class DialogBase : EventsBase, IDialog
    {
        private IDialogResult _lastResult = new DialogResult();

        /// <summary>
        /// ID from Strings.[id]
        /// </summary>
        protected abstract int Ok { get; } 

        /// <summary>
        /// ID from Strings.[id]
        /// </summary>
        protected abstract int Cancel { get; }

        protected abstract int Layout { get; }

        protected virtual Task<T> Show<T>(Context context, View view, Func<View,DialogClickEventArgsExtended, T> onClosing)
        {
            var result = new TaskCompletionSource<T>();

            Func<int, EventHandler<DialogClickEventArgs>> onClose =
                selection => (sender, args) =>
                {
                    result.SetResult(
                        onClosing(view, new 
                        DialogClickEventArgsExtended(args, selection == Ok)));                    
                };

            using (var builder = new AlertDialog.Builder(context)
                .SetPositiveButton(context.GetString(Ok), onClose(Ok))
                .SetNegativeButton(context.GetString(Cancel), onClose(Cancel)))
            {
                if (Layout > 0) OnBuilt(builder).SetView(view);
                // ...
                RaiseEvent(nameof(Show));
                builder.Show ();
            }
            return result.Task;
        }

        protected virtual AlertDialog.Builder OnBuilt(AlertDialog.Builder builder)
        {
            // builder.SetCustomTitle("Title")
            return builder;
        }

        public IDialogResult LastResult
        {
            get { return _lastResult; }
            private set
            {
                if (_lastResult == value) return;
                _lastResult = value;
                RaiseEvent(value);
            }
        }

        public virtual async Task<IDialogResult> Show(Activity activity, bool ignoreLastResult  = false)
        {
            if (LastResult.DontAskAgain && !ignoreLastResult)
            {
                return LastResult;
            }

            View dialogView = null;
            try
            {
                if (Layout > 0)
                {
                    dialogView = activity.LayoutInflater.Inflate(Layout, null);
                }

                LastResult = await Show(context: activity,view: dialogView,onClosing: OnClosing);

                return LastResult;
            }
            finally 
            {
                dialogView?.Dispose();
            }     
        }

        protected virtual IDialogResult OnClosing(View view, DialogClickEventArgsExtended args)
        {
            RaiseEvent();
            return new DialogResult(args.Ok);
        }
        private class DialogResult : IDialogResult
        {
            public DialogResult(bool ok = false, bool dontAskAgain = false)
            {
                DontAskAgain = dontAskAgain;
                Ok = ok;
            }

            public bool Ok { get; }
            public bool DontAskAgain { get; } 
        }
    }

    public static class DialogBaseExtensions
    {
        /// <summary>
        /// Can't exit
        /// </summary>        
        public static bool IsDeadLocked(this IDialog dialog)
        {
            return dialog != null && dialog.LastResult.DontAskAgain && !dialog.LastResult.Ok;
        }
    }
    
}
