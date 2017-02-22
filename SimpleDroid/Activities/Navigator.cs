using Android.Views;
using TinyIoC;

namespace SimpleDroid
{
    public class NavigatorFty
    {
        public NavigatorFty(TinyIoCContainer container)
        {
            Container = container;
        }

        TinyIoCContainer Container { get; }
        public INavigator Create(AppCompatActivityBase activity)
        {
            return new Navigator(activity, Container.Resolve<FragmentFactory>());
        }
    }
    public class Navigator: INavigator
    {
        private readonly FragmentFactory _fragmentFactory;

        private readonly AppCompatActivityBase _activity;

        public Navigator(AppCompatActivityBase activity, FragmentFactory fragmentFactory)
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
                    ft.Add(_activity.FragmentContainerID, fragment, navId.ToString())
                        .AddToBackStack(null);
                    ft.Commit();
                }
            }
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
}
