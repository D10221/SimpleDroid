using System;

namespace SimpleDroid
{
    public interface ICommand
    {
        bool CanExecute { get; }
        object Id { get; }
        void Execute(object parameter);

        event EventHandler CanExecuteChanged;
    }

    public class Command : ICommand
    {
        private Action<object> _action;

        private readonly Func<bool> _canExec;

        public Command(object id , Action<object> action, Func<bool> canExec = null)
        {
            Id = id;
            _action = action;
            _canExec = canExec ?? (() => true);
        }

        public bool CanExecute => _canExec();

        public virtual void Execute(object parameter)
        {
            _action.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public object Id { get; }
    }
}