using Prism.Mvvm;
using RSSFeeder.Services;
using System;
using System.Collections.ObjectModel;
using System.Timers;

namespace RSSFeeder.Models
{
    internal class FeedsModel : BindableBase
    {
        private DataService dataService;
        
        private ObservableCollection<Feed> _FeedTabs = new ObservableCollection<Feed>();
        public readonly ReadOnlyObservableCollection<Feed> FeedTabs;
        

        #region Singleton of FeedsModel
        private static FeedsModel instance;
        private FeedsModel()
        {
            dataService = new DataService();
            FeedTabs = new ReadOnlyObservableCollection<Feed>(_FeedTabs);

            if (App.IsDesignMode)
            {
                var newFeed = dataService.GetFeedByUrl(@"https://habr.com/rss/interesting/");
                _FeedTabs.Add(newFeed);
            }
            else if (Settings.UserURLs.Count != 0)
            {
                _RefreshData();
            }
            else
            {
                Feed newFeed = dataService.GetFeedByUrl(Settings.DefaultUrl);
                _FeedTabs.Add(newFeed);
            }
            
        }
        public static FeedsModel getInstance()
        {
            if (instance == null)
                instance = new FeedsModel();
            return instance;
        }
        #endregion

        #region Methods of Model
        public void AddFeed (string url)
        {
            var newFeed = dataService.GetFeedByUrl(url);
            _FeedTabs.Add(newFeed);
            Settings.UserURLs.Add(url);
            RaisePropertyChanged("FiltredFeedTabs");
        }
        public void RemoveFeed (Feed feed)
        {
            _FeedTabs.Remove(feed);
            Settings.UserURLs.Remove(feed.Link);
            RaisePropertyChanged("FiltredFeedTabs");
        }
        public void ChangeVisibleFeed (Feed feed)
        {
            var index = _FeedTabs.IndexOf(feed);
            _FeedTabs[index].Visible = !_FeedTabs[index].Visible;
            RaisePropertyChanged("FiltredFeedTabs");
        }
        public void RefreshData(object sender, ElapsedEventArgs e)
        {
            _RefreshData();
            RaisePropertyChanged("FiltredFeedTabs");
        }
        private void _RefreshData()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                _FeedTabs.Clear();
                foreach (var url in Settings.UserURLs)
                {
                    if (url is null) continue;
                    Feed newFeed = dataService.GetFeedByUrl(url);
                    _FeedTabs.Add(newFeed);
                }
            });
        }
        #endregion
    }
}
