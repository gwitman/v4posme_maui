using Posme.Maui.Views;
using static Microsoft.Maui.Controls.Application;

namespace Posme.Maui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        private bool _opcionPagar;
        private string _company;
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
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
        public string Company
        {
            get => this._company;
            set => SetProperty(ref this._company, value);
        }
        public bool OpcionPagar
        {
            get => this._opcionPagar;
            set => SetProperty(ref this._opcionPagar, value);
        }
        public Command LoginCommand { get; }


        async void OnLoginClicked()
        {
            //await Navigation.NavigateToAsync<AboutViewModel>(true);
            Current!.MainPage = new MainPage();
            Password = String.Empty;
        }

        bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(UserName)
                && !String.IsNullOrWhiteSpace(Password);
        }
    }
}
