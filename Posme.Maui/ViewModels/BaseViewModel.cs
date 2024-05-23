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

        public bool IsBusy
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                RaisePropertiesChanged();
            }
        }

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public event PropertyChangedEventHandler? PropertyChanged;


        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
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
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
