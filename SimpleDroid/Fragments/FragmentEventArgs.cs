namespace SimpleDroid
{
    public class FragmentEventArgs: IEventArgs
    {
        public FragmentEventArgs(string key, object value = null)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public object Value { get; }
    }
}