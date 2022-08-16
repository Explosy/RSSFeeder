using RSSFeeder.Models;
using RSSFeeder.Services;
using RSSFeeder.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;

namespace RSSFeeder.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        /// <summary>Коллекция лент для отображения вкладками в TabControl</summary>
        public ObservableCollection<Feed> FeedTabs { get; }
        private DataService dataService;
        private Timer timerForRefresh;


        public MainWindowViewModel()
        {
            dataService = new DataService();
            timerForRefresh = new Timer();
            
            FeedTabs = new ObservableCollection<Feed>();
            //УДАЛИТЬ DESIGNMODE ПОСЛЕ ЗАВЕРШЕНИЯ ПРИЛОЖЕНИЯ!!!!!!!!
            if (App.IsDesignMode)
            {
                var newFeed = dataService.GetFeedByUrl(@"https://habr.com/rss/interesting/");
                FeedTabs.Add(newFeed);
            }
            else if (!(Settings.UserUrls is null) && (Settings.UserUrls.Count != 0))
            {
                foreach (var url in Settings.UserUrls)
                {
                    if (url is null) continue;
                    var newFeed = dataService.GetFeedByUrl(url);
                    FeedTabs.Add(newFeed);
                }
            }
            else if (Settings.DefaultUrl!=null && !Equals(Settings.DefaultUrl, ""))
            {
                var newFeed = dataService.GetFeedByUrl(Settings.DefaultUrl);
                FeedTabs.Add(newFeed);
            }
            

            #region Commands

            OpenSettingsWindowCommand = new ActionCommand(OnOpenSettingsWindowCommandExecuted, CanOpenSettingsWindowCommandExecute);
            
            #endregion
        }

        #region Commands
        #region OpenSettingsWindowCommand
        public ICommand OpenSettingsWindowCommand { get; }
        private void OnOpenSettingsWindowCommandExecuted (object p)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
        private bool CanOpenSettingsWindowCommandExecute(object p) => true;
        #endregion
        
        


        #endregion

    }
}
