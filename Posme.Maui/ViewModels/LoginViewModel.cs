using System;
using Microsoft.Maui.Controls;

namespace Posme.Maui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string userName;
        string password;
        int idcompany;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }


        public string UserName
        {
            get => this.userName;
            set => SetProperty(ref this.userName, value);
        }

        public string Password
        {
            get => this.password;
            set => SetProperty(ref this.password, value);
        }

        public int Idcompany
        {
            get=> this.idcompany;
            set=> SetProperty(ref this.idcompany, value);
        }
        public Command LoginCommand { get; }


        async void OnLoginClicked()
        {
            await Navigation.NavigateToAsync<AboutViewModel>(true);
            Password = String.Empty;
        }

        bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(UserName)
                && !String.IsNullOrWhiteSpace(Password);
        }
    }
}
