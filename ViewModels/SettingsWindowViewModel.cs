using RSSFeeder.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RSSFeeder.ViewModels
{
    internal class SettingsWindowViewModel : BaseViewModel
    {

        #region Setting Fields
        public string DefaultUrl
        {
            get => Settings.DefaultUrl;
            set => Set(ref Settings.DefaultUrl, value);
        }
        public List<string> UserURLs
        {
            get => Settings.UserURLs;
            set => Set(ref Settings.UserURLs, value);
        }
        public uint UpdateTime
        {
            get => Settings.UpdateTime;
            set => Set(ref Settings.UpdateTime, value);
        }
        #endregion

        #region Forms Fields
        private string _SelectedURL;
        public string SelectedURL { get => _SelectedURL; set => Set(ref _SelectedURL, value); }

        #endregion

        private readonly FeedsModel _FeedsModel;
        public ReadOnlyObservableCollection<Feed> FeedTabs => _FeedsModel.FeedTabs;

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
        private bool CanCreateNewFeedCommandExecute(object p) => true;
        private void OnCreateNewFeedCommandExecuted(object p)
        {
            _FeedsModel.AddFeed(DefaultUrl);
        }
        #endregion
        #region DeleteFeedCommand
        public ICommand DeleteFeedCommand { get; }
        private bool CanDeleteFeedCommandExecute(object p) => p is string; //Feed feed && FeedTabs.Contains(feed);
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
