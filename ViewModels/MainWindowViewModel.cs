using System;


namespace RSSFeeder.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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
