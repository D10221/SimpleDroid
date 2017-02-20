namespace SimpleDroid
{
    public class ActivityEventArgs: IEventArgs
    {
        public ActivityEventArgs(string key, object value = null)
        {
            Key = key;
            Value = value;
        }

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