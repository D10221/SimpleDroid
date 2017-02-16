namespace SimpleDroid.Services.Remote
{
    class NetActionConfig : INetActionConfig
    {
        public string MethodName { get; set; }

        public object Parameters { get; set; }

        /// <summary>
        /// Body Payload
        /// </summary>
        public object Payload { get; set; }

        public string PayloadType { get; set; }
    }
}