using System.Threading.Tasks;

namespace SimpleDroid.Services.Remote
{
    public interface INetServiceRunner
    {
        Task<T> Invoke<T>(INetActionConfig action);
    }
}