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
               
        readonly FeedsModel _FeedsModel = new FeedsModel();

        private Timer timerForRefresh;


        public MainWindowViewModel()
        {
            
            timerForRefresh = new Timer();
            
            
            
            

            #region Commands

            OpenSettingsWindowCommand = new ActionCommand(OnOpenSettingsWindowCommandExecuted, CanOpenSettingsWindowCommandExecute);
            
            #endregion
        }

        public ReadOnlyObservableCollection<Feed> FeedTabs => _FeedsModel.FeedTabs;
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
