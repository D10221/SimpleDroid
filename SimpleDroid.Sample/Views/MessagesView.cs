namespace SimpleDroid.Views
{
    public class MessagesView : FragmentBase
    {
        public override int ToolbarMenuLayout { get; } = Resource.Layout.messages_toolbar_menu;
        protected override int FragmentLayout { get; } = Resource.Layout.messages_view;
        protected override bool HasOptionsMenu { get; } = true;
    }
}