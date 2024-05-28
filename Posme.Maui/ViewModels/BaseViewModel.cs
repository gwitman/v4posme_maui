using Posme.Maui.Models;
using Posme.Maui.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DevExpress.Maui.Core;

namespace Posme.Maui.ViewModels
{
    public class BaseViewModel : BindableBase, INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _title = string.Empty;
        protected AppMobileApiMGetDataDownloadItemsResponse _selectedItem;
        public bool IsBusy
        {
            get => GetValue<bool>();
            set
            {
                _isBusy = value;
                SetValue(value);
                RaisePropertiesChanged();
            }
        }

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string BarCode
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
                RaisePropertyChanged();
            }
        }

        public AppMobileApiMGetDataDownloadItemsResponse SelectedItem
        {
            get => GetValue<AppMobileApiMGetDataDownloadItemsResponse>();
            set
            {
                SetProperty(ref _selectedItem, value);
                SetValue(value);
                RaisePropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;


        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}