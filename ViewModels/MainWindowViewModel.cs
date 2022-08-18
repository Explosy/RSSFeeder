﻿using Prism.Mvvm;
using RSSFeeder.Models;
using RSSFeeder.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ReadOnlyObservableCollection<Feed> FiltredFeedTabs
        {
            get
            {
                return new ReadOnlyObservableCollection<Feed>(new ObservableCollection<Feed>(_FeedsModel.FeedTabs.Where(i => i.Visible)));
            }
        }

    public MainWindowViewModel()
        {
            _FeedsModel = FeedsModel.getInstance();
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
            OpenInWebCommand = new ActionCommand(OnOpenInWebCommandExecuted, CanOpenInWebCommandExecuted);
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

        #region OpenInWebCommand
        public ICommand OpenInWebCommand { get; }
        private void OnOpenInWebCommandExecuted(object p)
        {
            if (p is string url) Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }
        private bool CanOpenInWebCommandExecuted(object p) => true;
        #endregion
        #endregion

    }
}
