using Prism.Mvvm;
using RSSFeeder.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RSSFeeder.ViewModels
{
    internal class SettingsWindowViewModel : BindableBase
    {

        #region Setting Fields
        public string DefaultUrl
        {
            get => Settings.DefaultUrl;
            set => SetProperty(ref Settings.DefaultUrl, value);
        }
        public List<string> UserURLs
        {
            get => Settings.UserURLs;
            set => SetProperty(ref Settings.UserURLs, value);
        }
        public uint UpdateTime
        {
            get => Settings.UpdateTime;
            set => SetProperty(ref Settings.UpdateTime, value);
        }
        #endregion

        #region Forms Fields
        private Feed _SelectedURL;
        public Feed SelectedURL { get => _SelectedURL; set => SetProperty(ref _SelectedURL, value); }
        public ReadOnlyObservableCollection<Feed> FeedTabs => _FeedsModel.FeedTabs;
        private readonly FeedsModel _FeedsModel;
        #endregion


        public SettingsWindowViewModel()
        {
            _FeedsModel = FeedsModel.getInstance();

            #region Commands
            CreateNewFeedCommand = new ActionCommand(OnCreateNewFeedCommandExecuted, CanCreateNewFeedCommandExecute);
            DeleteFeedCommand = new ActionCommand(OnDeleteFeedCommandExecuted, CanDeleteFeedCommandExecute);
            ChangeVisibleFeedCommand = new ActionCommand(OnChangeVisibleFeedCommandExecuted, CanChangeVisibleFeedCommandExecute);
            #endregion

        }


        #region Commands

        #region CreateNewFeedCommand
        public ICommand CreateNewFeedCommand { get; }
        private bool CanCreateNewFeedCommandExecute(object p) => !Equals((string)p,"");
        private void OnCreateNewFeedCommandExecuted(object p)
        {
            var url = (string)p;
            if (!Equals(p,"")) _FeedsModel.AddFeed(url);
        }
        #endregion
        #region DeleteFeedCommand
        public ICommand DeleteFeedCommand { get; }
        private bool CanDeleteFeedCommandExecute(object p) => p is Feed feed && FeedTabs.Contains(feed);
        private void OnDeleteFeedCommandExecuted(object p)
        {
            if (!(p is Feed feed)) return;
            _FeedsModel.RemoveFeed(feed);
        }
        #endregion
        #region ChangeVisibleFeedCommand
        public ICommand ChangeVisibleFeedCommand { get; }
        private bool CanChangeVisibleFeedCommandExecute(object p) => true;
        private void OnChangeVisibleFeedCommandExecuted(object p)
        {
            if (!(p is Feed feed)) return;
            feed.Visible = !feed.Visible;
        }
        #endregion

        #endregion
    }
}
