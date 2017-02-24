using Android.Support.V4.App;
using NLog;
using TinyIoC;

namespace SimpleDroid
{
    public interface IFragmentFactory
    {
        Fragment Resolve(int menuItemItemId);
    }

    public class FragmentFactory : IFragmentFactory
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly TinyIoC.TinyIoCContainer _container;

        public FragmentFactory(TinyIoCContainer container)
        {
            _container = container;
        }

        public Fragment Resolve(int menuItemItemId)
        {
            Fragment fragment;
            if (_container.TryResolve<Fragment>(menuItemItemId.ToString(), out fragment))
            {
                _logger.Debug($"Unable to resolve Fragment named as :{menuItemItemId}");
            }
            return fragment;
        }
    }
}