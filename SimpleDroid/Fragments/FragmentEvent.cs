namespace SimpleDroid
{
    public class FragmentEvent: IEvent
    {
        public FragmentEvent(string key, object value = null)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public object Value { get; }
    }
}