
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Views;
using Android.Widget;

namespace SimpleDroid.Views
{
    public class HomeView : FragmentBase, INotifyPropertyChanged
    {
        public override int MenuLayout { get; } = Resource.Layout.home_view_toolbar_menu;        
        protected override int FragmentLayout { get; } = Resource.Layout.home_view;
        public List<ICommand> Commands { get; set; }
        
        /// <summary>
        /// view doesn't exists on OnCreate
        /// </summary>
        protected override void OnViewCreated(View view)
        {
            Commands = new List<ICommand>()
            {
                new Command(Resource.Id.home_view_menu_say_hello, SayHello(view), CanSayHello(view))
            };

            CommandExtensions
                .BindCommands(
                    menu: Menu, 
                    source: this,
                    commandsMember: v => v.Commands,
                    menuItemClicks: this.WhenMenuItemClick())
                .ToBeDisposedBy(this);
        }

        private Func<bool> CanSayHello(View view)
        {
            return  () => ! view.FindViewById<TextView>(Resource.Id.home_view_text)
            .Text
            .Equals(GetText(Resource.String.Hello));
        }

        private Action<object> SayHello(View view)
        {
            return x =>
            {
                view.FindViewById<TextView>(Resource.Id.home_view_text).Text = GetText(Resource.String.Hello);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}