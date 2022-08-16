using RSSFeeder.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RSSFeeder.ViewModels
{
    internal class SettingsWindowViewModel : BaseViewModel
    {
        #region Setting Fields
        private string _DefaultUrl;
        public string DefaultUrl {
            get => _DefaultUrl;
            set => Set(ref _DefaultUrl, value);
        }

        private List<string> _UserURLs;
        public List<string> UserURLs
        {
            get => _UserURLs;
            set => Set(ref _UserURLs, value);
        }

        private uint _UpdateTime;
        public uint UpdateTime
        {
            get => _UpdateTime;
            set => Set(ref _UpdateTime, value);
        }
        #endregion

        #region Forms Fields
        private string _SelectedURL;
        public string SelectedURL { get => _SelectedURL; set => Set(ref _SelectedURL, value); }

        #endregion

        public SettingsWindowViewModel()
        {
            DefaultUrl = Settings.DefaultUrl;
            UserURLs = Settings.UserUrls;
            UpdateTime = Settings.UpdateTime;
        }

        public void SaveSettings(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.DefaultUrl = DefaultUrl;
            Settings.UserUrls = UserURLs;
            Settings.UpdateTime = UpdateTime;
        }

        //#region Commands

        //#region CreateNewFeed
        //public ICommand CreateNewFeed { get; }
        //private bool CanCreateNewFeedCommandExecute(object p) => true;
        //private void OnCreateNewFeedCommandExecuted(object p)
        //{
        //    var newFeed = dataService.GetFeedByUrl(Settings.DefaultUrl);
        //    FeedTabs.Add(newFeed);
        //}
        //#endregion
        //#region DeleteFeedCommand
        //public ICommand DeleteFeed { get; }
        //private bool CanDeleteFeedCommandExecute(object p) => p is Feed feed && FeedTabs.Contains(feed);
        //private void OnDeleteFeedCommandExecuted(object p)
        //{
        //    if (!(p is Feed feed)) return;
        //    FeedTabs.Remove(feed);
        //}
        //#endregion

        //#endregion
    }
}
