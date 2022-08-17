using RSSFeeder.Models;
using System.Windows;

namespace RSSFeeder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static bool IsDesignMode { get; private set; } = true;

        protected override void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;
            Settings.Load();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Save();
            base.OnExit(e);
        }

    }
}
