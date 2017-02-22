using Android.Support.V4.App;
using TinyIoC;

namespace SimpleDroid
{
    public interface IFragmentFactory
    {
        Fragment Resolve(int menuItemItemId);
    }

    public class FragmentFactory : IFragmentFactory
    {
        private readonly TinyIoC.TinyIoCContainer _container;

        public FragmentFactory(TinyIoCContainer container)
        {
            _container = container;
        }

        public Fragment Resolve(int menuItemItemId)
        {
            return _container.Resolve<Fragment>(menuItemItemId.ToString());
        }
    }
}