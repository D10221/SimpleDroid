using System.Collections.Generic;

namespace SimpleDroid.Services.Remote
{
    public interface INetService
    {
        IList<INetActionConfig> Actions { get; }
        INetServiceConfig Config { get; }
    }
}