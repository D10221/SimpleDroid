using System.Collections.Generic;

namespace SimpleDroid.Services.Remote
{
    class NetService : INetService
    {
        public INetServiceConfig Config { get; }

        public NetService(INetServiceConfig config, IList<INetActionConfig> actions = null)
        {
            if (actions != null) Actions = actions;
            Config = config;
        }
        public IList<INetActionConfig> Actions { get; private set; } = new List<INetActionConfig>();
    }
}