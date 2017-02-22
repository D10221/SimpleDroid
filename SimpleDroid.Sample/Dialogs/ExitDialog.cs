using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace SimpleDroid.Dialogs
{
    internal class ExitDialog : DialogBase
    {
        protected override int Ok { get; } = Resource.String.ok;
        protected override int Cancel { get; } = Resource.String.cancel;
        protected override int Layout { get; } = Resource.Layout.exit_prompt_dialog;

        protected override IDialogResult OnClosing(View view, DialogClickEventArgsExtended args)
        {
            return new ConfirmExitDialogResult
            {                
                DontAskAgain = view.FindViewById<CheckBox>(Resource.Id.exit_prompt_dontaskagain).Checked,
                Ok = args.Ok
            };
        }
        protected override AlertDialog.Builder OnBuilt(AlertDialog.Builder builder)
        {            
            builder.SetMessage(Resource.String.exit_prompt_message);
            return builder;
        }

        class ConfirmExitDialogResult : IDialogResult
        {
            public bool Ok { get; set; }
            public bool DontAskAgain { get; set; }
        }
    }
}