using Android.App;
using Android.OS;
using Android.Views;

namespace SimpleDroid
{
    public abstract class FragmentBase : Fragment
    {
        protected abstract int FragmentLayout { get; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            SetHasOptionsMenu(HasOptionsMenu);
            base.OnCreate(savedInstanceState);
        }

        protected abstract bool HasOptionsMenu { get; }
        
        public virtual int ToolbarMenuLayout { get; } = 0;
        
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            if (ToolbarMenuLayout > 0)
            {
                if (menu != null)
                {
                    menu.Clear();

                    inflater.Inflate(ToolbarMenuLayout, menu);

                    OnMenuInflated(menu);
                }
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        protected virtual void OnMenuInflated(IMenu menu)
        {
            //menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
            //menu.FindItem(Resource.Id.action_attach).SetVisible(false);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(FragmentLayout, container, false);
            // base.OnCreateView(inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState)
            return view;
        }
    }
}