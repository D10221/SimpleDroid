namespace SimpleDroid
{
    public class ActivityEvent: IEvent
    {
        public ActivityEvent(string key, object value = null)
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