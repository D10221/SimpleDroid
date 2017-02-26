using Android.Views;
using NLog;
using TinyIoC;

namespace SimpleDroid
{
    public interface INavigatorFty
    {
        INavigator Create(ActivityBase activity);
    }

    public class NavigatorFty: INavigatorFty
    {
        public NavigatorFty(TinyIoCContainer container)
        {
            Container = container;
        }

        TinyIoCContainer Container { get; }
        public INavigator Create(ActivityBase activity)
        {
            return new FragmentNavigator(activity, Container.Resolve<FragmentFactory>());
        }
    }

    public interface INavigator
    {
        void Navigate(IMenuItem menuItem);
        
        /// <summary>
        /// MenuItem Id
        /// </summary>
        /// <param name="navId"></param>
        void Navigate(int navId);
    }

    public class FragmentNavigator: INavigator
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IFragmentFactory _fragmentFactory;

        private readonly ActivityBase _activity;

        public FragmentNavigator(ActivityBase activity, IFragmentFactory fragmentFactory)
        {
            _activity = activity;
            _fragmentFactory = fragmentFactory;
        }

        public virtual void Navigate(IMenuItem menuItem)
        {
            Navigate(menuItem.ItemId);
        }

        public virtual void Navigate(int navId)
        {
            var fragmentManager = _activity.CurrentFragmentManager;

            if (fragmentManager != null && _activity.FragmentContainerID > 0 && navId > 0)
            {
                using (var ft = fragmentManager.BeginTransaction())
                {
                    var fragment = _fragmentFactory.Resolve(navId);
                    if (fragment == null)
                    {
                        _logger.Debug($"No Fragment for NavId: {navId}");
                        return; 
                    }

                    ft.Add(_activity.FragmentContainerID, fragment, navId.ToString())
                        .AddToBackStack(null);
                    ft.Commit();
                }
            }
        }
    }
}
