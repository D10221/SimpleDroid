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

    public interface IDialog
    {
        Task<IDialogResult> Show(Activity activity);
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
    public abstract class DialogBase : IDialog
    {       
        /// <summary>
        /// ID from Strings.[id]
        /// </summary>
        protected abstract int Yes { get; } 

        /// <summary>
        /// ID from Strings.[id]
        /// </summary>
        protected abstract int No { get; }

        protected abstract int Layout { get; }

        protected virtual Task<T> Show<T>(Context context, View view, Func<View,DialogClickEventArgsExtended, T> onClosing)
        {
            var result = new TaskCompletionSource<T>();

            Func<int, EventHandler<DialogClickEventArgs>> onClose =
                selection => (sender, args) =>
                {
                    result.SetResult(
                        onClosing(view, new DialogClickEventArgsExtended(args, selection == Yes)));
                };

            using (var builder = new AlertDialog.Builder(context)
                .SetPositiveButton(context.GetString(Yes), onClose(Yes))
                .SetNegativeButton(context.GetString(No), onClose(No)))
            {
                if (Layout > 0) builder.SetView(view);  
                 // ...
                 builder.Show ();
            }
            return result.Task;
        }
        public IDialogResult LastResult { get; private set; }
        public virtual async Task<IDialogResult> Show(Activity activity)
        {
            if (LastResult?.DontAskAgain??false)
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
            return new DialogResult(args.Ok);

        }
        private class DialogResult : IDialogResult
        {
            public DialogResult(bool ok)
            {
                Ok = ok;
            }

            public bool Ok { get; }
            public bool DontAskAgain { get; } = false;
        }
    }
    
}
