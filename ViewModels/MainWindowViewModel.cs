using Prism.Mvvm;
using RSSFeeder.Models;
using RSSFeeder.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;

namespace RSSFeeder.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {

        private readonly FeedsModel _FeedsModel;
        private Timer timerForRefresh;
        public ReadOnlyObservableCollection<Feed> FeedTabs => _FeedsModel.FeedTabs;

        public MainWindowViewModel()
        {
            _FeedsModel = new FeedsModel();
            _FeedsModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            #region Timer settings
            timerForRefresh = new Timer();
            timerForRefresh.AutoReset = true;
            timerForRefresh.Interval = Settings.UpdateTime * 1000;
            timerForRefresh.Elapsed += _FeedsModel.RefreshData;
            timerForRefresh.Enabled = true;
            #endregion

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
