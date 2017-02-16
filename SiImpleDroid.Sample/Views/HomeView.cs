namespace SimpleDroid.Views
{
    public class HomeView : FragmentBase
    {
        public override int ToolbarMenuLayout { get; } = Resource.Layout.home_view_toolbar_menu;
        protected override bool HasOptionsMenu { get; } = true;
        protected override int FragmentLayout { get; } = Resource.Layout.home_view;      
    }
}