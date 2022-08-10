using System;
using System.Windows.Input;

namespace RSSFeeder
{
    internal abstract class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract void Execute(object? parameter);
        public abstract bool CanExecute(object? parameter);
    }
}
