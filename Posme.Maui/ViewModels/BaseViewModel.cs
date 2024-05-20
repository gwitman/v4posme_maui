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

        public INavigationService Navigation => DependencyService.Get<INavigationService>();

        public bool IsBusy
        {
            get => this._isBusy;
            set => SetProperty(ref this._isBusy, value);
        }

        public string Title
        {
            get => this._title;
            set => SetProperty(ref this._title, value);
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
