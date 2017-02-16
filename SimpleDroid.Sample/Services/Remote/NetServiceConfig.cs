namespace SimpleDroid.Services.Remote
{
    class NetServiceConfig : INetServiceConfig
    {
        public string ServiceName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceHost { get; set; }
        public string ServicePort { get; set; }
        public string NameSpace { get; set; }
        
        public ServiceDataMode DataMode { get; } = ServiceDataMode.None;
        public bool HasCredentials => !string.IsNullOrWhiteSpace(UserName);
        public string ServiceBaseUri =>
            $"http://{ServiceHost}:{ServicePort}/" +
            $"{(string.IsNullOrWhiteSpace(NameSpace) ? "" : $"{NameSpace}/")}" +
            $"{DataMode}";
    }
}