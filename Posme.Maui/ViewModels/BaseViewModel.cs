using Posme.Maui.Models;
using Posme.Maui.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DevExpress.Maui.Core;

namespace Posme.Maui.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        private bool _isBusy;
        private string _title = string.Empty;
        private string _search = string.Empty;

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

        public string Search
        {
            get => GetValue<string>();
            set
            {
                SetProperty(ref _search, value);
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