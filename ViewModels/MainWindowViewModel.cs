using RSSFeeder.Models;
using System;
using System.Collections.ObjectModel;

namespace RSSFeeder.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ObservableCollection<RssItem> Items { get; }
        
        public MainWindowViewModel()
        {
            RssFeed rssFeed = new RssFeed("https://habr.com/rss/interesting/");
            Items = new ObservableCollection<RssItem>(rssFeed.Items);
        }

        
        private string _Title = "RSSFeeder";
        
        public string Title
        {
            get => _Title;
            set
            {
                if (Equals(_Title, value)) return;
                _Title = value;
                OnPropertyChanged();
            }
        }
    }
}
