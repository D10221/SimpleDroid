namespace SimpleDroid
{
    public class ActivityEventArgs
    {
        public ActivityEventArgs(object sender, string key, object value = null)
        {
            Sender = sender;
            Key = key;
            Value = value;
        }

        public object Sender { get; }
        public string Key { get; }

        public object Value { get; }
    }

    public enum ViewState
    {
        Restarting,
        Resuming,
        Starting,
        Destroying,
        Pausing,
        Stopping
    }
}