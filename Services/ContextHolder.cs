using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RSSFeeder.Services
{
    internal class ContextHolder : ContentControl
    {
        private readonly Dictionary<ICommand, CommandBinding> _dict;

        public CommandsCollection Commands
        {
            get { return (CommandsCollection)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands", typeof(CommandsCollection), typeof(ContextHolder), new PropertyMetadata(null));

        public ContextHolder()
        {
            Commands = new CommandsCollection();
            Commands.Changed += FreezableCollectionChanged;
            _dict = new Dictionary<ICommand, CommandBinding>();

            CommandManager.AddCanExecuteHandler(this, CanExecute);
            CommandManager.AddExecutedHandler(this, OnExecute);
        }

        private void FreezableCollectionChanged(object sender, EventArgs e)
        {
            _dict.Clear();
            foreach (var i in Commands) { _dict.Add(i.RoutedCommand, i); }
        }


        private void OnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dict.TryGetValue(e.Command, out CommandBinding binding))
            {
                if (binding?.RelayCommand != null) binding.RelayCommand.Execute(e.Parameter);
            }
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dict.TryGetValue(e.Command, out CommandBinding binding))
            {
                if (binding?.RelayCommand != null) e.CanExecute = binding.RelayCommand.CanExecute(e.Parameter);
            }
        }
    }
    internal class CommandsCollection : FreezableCollection<CommandBinding> { }
    internal class CommandBinding : Freezable
    {
        public ICommand RoutedCommand
        {
            get { return (ICommand)GetValue(RoutedCommandProperty); }
            set { SetValue(RoutedCommandProperty, value); }
        }
        public static readonly DependencyProperty RoutedCommandProperty =
            DependencyProperty.Register("RoutedCommand", typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));


        public ICommand RelayCommand
        {
            get { return (ICommand)GetValue(RelayCommandProperty); }
            set { SetValue(RelayCommandProperty, value); }
        }
        public static readonly DependencyProperty RelayCommandProperty =
            DependencyProperty.Register("RelayCommand", typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));

        protected override Freezable CreateInstanceCore()
        {
            return new CommandBinding();
        }
    }
}
