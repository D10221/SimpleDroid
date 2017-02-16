namespace SimpleDroid.Services.Remote
{
    public interface INetServiceConfig
    {
        string UserName { get; }
        string Password { get; }
        string ServiceHost { get; }
        string ServicePort { get; }
        string NameSpace { get; }
        ServiceDataMode DataMode { get; }
        bool HasCredentials { get; }
        string ServiceBaseUri { get; }
        string ServiceName { get; }
    }
}