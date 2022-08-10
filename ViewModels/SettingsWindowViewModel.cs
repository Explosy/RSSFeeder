using System;


namespace RSSFeeder.ViewModels
{
    internal class SettingsWindowViewModel : ViewModel
    {
        private string _DefaultUrl;
        public string DefaultUrl {
            get => _DefaultUrl;
            set
            {
                if (Equals(_DefaultUrl, value)) return;
                _DefaultUrl = value;
                OnPropertyChanged();
            }
        }
        public SettingsWindowViewModel()
        {
            DefaultUrl = @"https://habr.com/rss/interesting/";
        }
    }
}
