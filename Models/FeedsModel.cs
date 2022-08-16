using RSSFeeder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RSSFeeder.Models
{
    internal class FeedsModel
    {
        private readonly ObservableCollection<Feed> _FeedTabs = new ObservableCollection<Feed>();
        public readonly ReadOnlyObservableCollection<Feed> FeedTabs;

        private DataService dataService;

        public FeedsModel()
        {
            dataService = new DataService();

            if (App.IsDesignMode)
            {
                var newFeed = dataService.GetFeedByUrl(@"https://habr.com/rss/interesting/");
                _FeedTabs.Add(newFeed);
            }
            else if (!(Settings.UserUrls is null) && (Settings.UserUrls.Count != 0))
            {
                foreach (var url in Settings.UserUrls)
                {
                    if (url is null) continue;
                    Feed newFeed = dataService.GetFeedByUrl(url);
                    _FeedTabs.Add(newFeed);
                }
            }
            else if (Settings.DefaultUrl != null && !Equals(Settings.DefaultUrl, ""))
            {
                Feed newFeed = dataService.GetFeedByUrl(Settings.DefaultUrl);
                _FeedTabs.Add(newFeed);
            }

            FeedTabs = new ReadOnlyObservableCollection<Feed>(_FeedTabs);
        }

        public void AddFeed (string url)
        {
            var newFeed = dataService.GetFeedByUrl(url);
            _FeedTabs.Add(newFeed);
        }
        public void RemoveFeed (Feed feed)
        {
            _FeedTabs.Remove(feed);
        }

        private void RefreshData()
        {

        }
    }
}
