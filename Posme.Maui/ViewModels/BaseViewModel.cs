using DevExpress.Maui.Core;

namespace Posme.Maui.ViewModels
{
    public abstract class BaseViewModel : BindableBase
    {
        private bool _isBusy;
        private string _title = string.Empty;
        private string _search = string.Empty;
        private string? _barCode;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetValue(ref _isBusy, value, () => RaisePropertyChanged(nameof(IsBusy)));
        }

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(ref _title, value, () => RaisePropertyChanged());
        }

        public string? BarCode
        {
            get => _barCode;
            set => SetValue(ref _barCode, value, () => RaisePropertyChanged(nameof(BarCode)));
        }

        public string Search
        {
            get => _search;
            set => SetValue(ref _search, value, () => RaisePropertyChanged());
        }

        private INavigation? _navigation;

        public INavigation? Navigation
        {
            get => _navigation;
            set => SetValue(ref _navigation, value, () => RaisePropertyChanged(nameof(Navigation)));
        }
    }
}