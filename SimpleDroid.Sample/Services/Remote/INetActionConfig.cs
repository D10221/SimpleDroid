namespace SimpleDroid.Services.Remote
{
    public interface INetActionConfig
    {
        string MethodName { get; }
        object Parameters { get; }
        object Payload { get; }
        string PayloadType { get; set; }
    }
}