using System;
using Android.Support.Design.Widget;

namespace SimpleDroid
{
    public class SnackbarCallback : Snackbar.Callback
    {
        private readonly Action<Snackbar> _onDimissedd;
        private readonly Action<Snackbar> _onShown;

        public SnackbarCallback(Action<Snackbar> onDimissedd = null, Action<Snackbar> onShown = null)
        {
            _onDimissedd = onDimissedd;
            _onShown = onShown;
        }

        public override void OnDismissed(Snackbar snackbar, int evt)
        {
            base.OnDismissed(snackbar, evt);
            _onDimissedd?.Invoke(snackbar);
        }

        public override void OnShown(Snackbar snackbar)
        {
            base.OnShown(snackbar);
            _onShown?.Invoke(snackbar);
        }
    }
}