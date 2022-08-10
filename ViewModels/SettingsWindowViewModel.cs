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
                Properties.Settings.Default.DefaultUrl = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }
        public SettingsWindowViewModel()
        {
            DefaultUrl = Properties.Settings.Default.DefaultUrl;
        }
    }
}
