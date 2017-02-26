namespace SimpleDroid.Views
{
    public class MessagesView : FragmentBase
    {
        public override int MenuLayout { get; } = Resource.Layout.messages_toolbar_menu;
        protected override int FragmentLayout { get; } = Resource.Layout.messages_view;        
    }
}