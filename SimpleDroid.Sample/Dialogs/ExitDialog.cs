using Android.Views;
using Android.Widget;

namespace SimpleDroid.Dialogs
{
    internal class ExitDialog : DialogBase
    {
        protected override int Yes { get; } = Resource.String.yes;
        protected override int No { get; } = Resource.String.no;
        protected override int Layout { get; } = Resource.Layout.exit_prompt_dialog;

        protected override IDialogResult OnClosing(View view, DialogClickEventArgsExtended args)
        {
            return new ConfirmExitDialogResult
            {
                DontAskAgain = view.FindViewById<CheckBox>(Resource.Id.exit_prompt_dontaskagain).Checked,
                Ok = args.Ok
            };
        }        

      class ConfirmExitDialogResult : IDialogResult
        {
            public bool Ok { get; set; }
            public bool DontAskAgain { get; set; }
        }
    }
}