using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Posme.Maui.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _title = string.Empty;
        private string _search = string.Empty;
        private string? _barCode;

        public bool IsBusy
        {
            get => _isBusy;
            protected set => SetProperty(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string? BarCode
        {
            get => _barCode;
            set => SetProperty(ref _barCode, value);
        }

        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        private string? _mensaje;

        public string? Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        private INavigation? _navigation;

        protected INavigation? Navigation
        {
            get => _navigation;
            set => SetProperty(ref _navigation, value);
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}