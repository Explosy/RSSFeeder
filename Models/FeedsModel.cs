using Prism.Mvvm;
using RSSFeeder.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
                LoadData();
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
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                int index = 0;
                foreach (var url in Settings.UserURLs)
                {
                    var visible = _FeedTabs[index].Visible;
                    _FeedTabs[index] = dataService.GetFeedByUrl(url);
                    _FeedTabs[index].Visible = visible;
                    index++;
                }
            });
            RaisePropertyChanged("FiltredFeedTabs");
        }
        private void LoadData()
        {
            foreach (var url in Settings.UserURLs)
            {
                if (url is null) continue;
                Feed newFeed = dataService.GetFeedByUrl(url);
                _FeedTabs.Add(newFeed);
            }
        }
        #endregion
    }
}
