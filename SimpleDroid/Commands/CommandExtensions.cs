using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Android.Views;

namespace SimpleDroid
{
    public static class CommandExtensions 
    {
        public static ICommand FindCommand(this IEnumerable<ICommand> commands, object id)
        {
            return commands.FirstOrDefault(cmd => cmd.Id.Equals(id));
        }

        public static Action<IMenu> SyncMenuState(this IList<ICommand> commands)
        {
            return menu =>
            {
                if (menu == null) return;

                foreach (var item in menu.Items())
                {
                    SyncMenuState(commands.FindCommand(item.ItemId), item);
                }
            };
        }

        private static IMenuItem SyncMenuState(this ICommand cmd, IMenuItem item)
        {
            return item.SetEnabled(cmd?.CanExecute ?? true);
        }

        public static IDisposable BindCommands<T, TR>(
            IMenu menu,
            T source,
            Expression<Func<T, TR>> commandsMember,
            IObservable<IMenuItem> menuItemClicks)
            where TR : IList<ICommand>
            where T : INotifyPropertyChanged
        {
            var getter = commandsMember.Compile();

            var commands = getter.Invoke(source);

            var cmdClick = menuItemClicks.Subscribe(InvokeCommand(commands));

            var cmdChanged =
                source.WhenPropertyChanged((commandsMember.Body as MemberExpression)?.Member.Name)
                .Select(x => menu)
                .Subscribe(commands.SyncMenuState());


            return new Subscription(() =>
            {
                cmdClick?.Dispose();
                cmdChanged?.Dispose();
            });
        }

        public static Action<IMenuItem> InvokeCommand(this IList<ICommand> commands)
        {
            return e =>
            {
                if (commands == null) return;

                var command = commands.FirstOrDefault(cmd => cmd.Id.Equals(e.ItemId));
                if (command?.CanExecute ?? false)
                {
                    command?.Execute(null);
                }
            };
        }
    }
}