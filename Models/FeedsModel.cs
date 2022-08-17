using Prism.Mvvm;
using RSSFeeder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace RSSFeeder.Models
{
    internal class FeedsModel : BindableBase//INotifyPropertyChanged
    {
        private DataService dataService;
        
        private readonly ObservableCollection<Feed> _FeedTabs = new ObservableCollection<Feed>();
        public readonly ReadOnlyObservableCollection<Feed> FeedTabs;

        #region Singleton of FeedsModel
        private static FeedsModel instance;
        public FeedsModel()
        {
            dataService = new DataService();
            

            if (App.IsDesignMode)
            {
                var newFeed = dataService.GetFeedByUrl(@"https://habr.com/rss/interesting/");
                _FeedTabs.Add(newFeed);
            }
            else if (Settings.UserURLs.Count != 0)
            {
                //RefreshData();
            }
            else
            {
                Feed newFeed = dataService.GetFeedByUrl(Settings.DefaultUrl);
                _FeedTabs.Add(newFeed);
            }
            FeedTabs = new ReadOnlyObservableCollection<Feed>(_FeedTabs);
        }

        public static FeedsModel getInstance()
        {
            if (instance == null)
                instance = new FeedsModel();
            return instance;
        }
        #endregion

        //public FeedsModel()
        //{
        //    dataService = new DataService();

        //    if (App.IsDesignMode)
        //    {
        //        var newFeed = dataService.GetFeedByUrl(@"https://habr.com/rss/interesting/");
        //        _FeedTabs.Add(newFeed);
        //    }
        //    else if (!(Settings.UserUrls is null) && (Settings.UserUrls.Count != 0))
        //    {
        //        RefreshData();
        //    }
        //    else if (Settings.DefaultUrl != null && !Equals(Settings.DefaultUrl, ""))
        //    {
        //        Feed newFeed = dataService.GetFeedByUrl(Settings.DefaultUrl);
        //        _FeedTabs.Add(newFeed);
        //    }

        //    FeedTabs = new ReadOnlyObservableCollection<Feed>(_FeedTabs);
        //}

        #region Methods of Model
        public void AddFeed (string url)
        {
            var newFeed = dataService.GetFeedByUrl(url);
            _FeedTabs.Add(newFeed);
        }
        public void RemoveFeed (Feed feed)
        {
            _FeedTabs.Remove(feed);
        }
        public void RefreshData(object sender, ElapsedEventArgs e)
        {
            _FeedTabs.Clear();
            foreach (var url in Settings.UserURLs)
            {
                if (url is null) continue;
                Feed newFeed = dataService.GetFeedByUrl(url);
                _FeedTabs.Add(newFeed);
            }
        }

        #endregion

        #region INotifyPropertyChanged
        //public event PropertyChangedEventHandler? PropertyChanged;
        //protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        //}
        #endregion
    }
}
