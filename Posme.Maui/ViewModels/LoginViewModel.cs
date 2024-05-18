using System.Windows.Input;
using DevExpress.Maui.Controls;
using Posme.Maui.Services.Helpers;
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
        private bool _popupShow;
        private string _mensaje = string.Empty;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            MensajeCommand = new Command(OnMensaje, ValidateError);
            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }
        public Command LoginCommand { get; }
        public Command MensajeCommand { get; }
        public string Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        public bool PopupShow
        {
            get => _popupShow;
            set => SetProperty(ref _popupShow, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref this._userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref this._password, value);
        }

        public string Company
        {
            get => _company;
            set => SetProperty(ref this._company, value);
        }

        public bool OpcionPagar
        {
            get => _opcionPagar;
            set => SetProperty(ref this._opcionPagar, value);
        }

       private async void OnLoginClicked()
        {
            /*VariablesGlobales.CompanyKey = Company.ToLower();
            var restServiceUser = new RestServiceUser();
            PopupShow = await restServiceUser.FindUser(UserName, Password);
            if (!PopupShow)
            {
                await Current!.MainPage!.DisplayAlert("Inicio Sesion", Mensaje, "OK");
                return;
            }

            Password = string.Empty;
            Current!.MainPage = new MainPage();*/
        }

        private void OnMensaje()
        {
            Mensaje = PopupShow ? "" : @"Credenciales incorrectas o nombre de compañía no existe. Inténtalo nuevamente.";
        }

        private bool ValidateError()
        {
            return PopupShow;
        }

        private bool ValidateLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName)
                   && !string.IsNullOrWhiteSpace(Password)
                   && !string.IsNullOrWhiteSpace(Company)
                   && UserName.Length > 3
                   && Password.Length > 3
                   && Company.Length > 3;
        }
    }
}