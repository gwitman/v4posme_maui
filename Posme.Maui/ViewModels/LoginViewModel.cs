using System;
using Microsoft.Maui.Controls;

namespace Posme.Maui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        private bool _opcionPagar;
        private int _idcompany;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        public Command PagarCheckCommand { get; set; }

        public bool OpcionPagar
        {
            get => _opcionPagar;
            set => SetProperty(ref _opcionPagar, value);
        }

        public string UserName
        {
            get => this._userName;
            set => SetProperty(ref this._userName, value);
        }

        public string Password
        {
            get => this._password;
            set => SetProperty(ref this._password, value);
        }

        public int Idcompany
        {
            get=> this._idcompany;
            set=> SetProperty(ref this._idcompany, value);
        }
        public Command LoginCommand { get; }
        
        private async void OnLoginClicked()
        {
            await Navigation.NavigateToAsync<AboutViewModel>(true);
            Password = string.Empty;
        }
        
        private bool ValidateLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName)
                && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
