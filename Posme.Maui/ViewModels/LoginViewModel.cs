using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;
using static Microsoft.Maui.Controls.Application;

namespace Posme.Maui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly RestApiCoreAcount _restServiceUser = new();
        private readonly IRepositoryTbUser _repositoryTbUser;
        private string _userName;
        private string _password;
        private bool _opcionPagar;
        private string _company;
        private bool _popupShow;
        private string _mensaje;
        private bool _remember;
        private INavigation? _navigation;

        public LoginViewModel()
        {
            _repositoryTbUser = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbUser>();
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            MensajeCommand = new Command(OnMensaje, ValidateError);
            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        public Command LoginCommand { get; }
        public Command MensajeCommand { get; }

        public string Mensaje
        {
            get => _mensaje;
            set => SetValue(ref _mensaje, value, () => RaisePropertyChanged());
        }

        public bool PopupShow
        {
            get => _popupShow;
            set => SetValue(ref _popupShow, value, () => RaisePropertyChanged(nameof(PopupShow)));
        }

        public string UserName
        {
            get => _userName;
            set => SetValue(ref this._userName, value, () => RaisePropertyChanged());
        }

        public string Password
        {
            get => _password;
            set => SetValue(ref this._password, value, () => RaisePropertyChanged());
        }

        public string Company
        {
            get => _company;
            set => SetValue(ref _company, value, () => RaisePropertyChanged());
        }

        public bool Remember
        {
            get => _remember;
            set => SetValue(ref _remember, value, () => RaisePropertyChanged(nameof(Remember)));
        }

        public bool OpcionPagar
        {
            get => _opcionPagar;
            set => SetValue(ref this._opcionPagar, value, () => RaisePropertyChanged(nameof(OpcionPagar)));
        }

        private async void OnLoginClicked()
        {
            await _navigation!.PushModalAsync(new LoadingPage());
            VariablesGlobales.CompanyKey = Company.ToLower();
            var findUserRemember =
                await _repositoryTbUser!.PosMeFindUserByNicknameAndPassword(UserName, Password);
            if (Remember)
            {
                var response = await _restServiceUser.LoginMobile(UserName, Password);
                if (!response)
                {
                    Mensaje = Mensajes.MensajeCredencialesInvalida;
                    PopupShow = true;
                    await _navigation.PopModalAsync();
                    return;
                }
                else
                {
                    PopupShow = false;
                }

                await _repositoryTbUser.PosMeOnRemember();
                if (findUserRemember is not null)
                {
                    findUserRemember.Remember = true;
                    findUserRemember.Company = Company;
                    await _repositoryTbUser.PosMeUpdate(findUserRemember);
                }
                else
                {
                    VariablesGlobales.User!.Company = Company;
                    VariablesGlobales.User.Remember = true;
                    await _repositoryTbUser.PosMeInsert(VariablesGlobales.User);
                }
            }
            else
            {
                if (await _repositoryTbUser.PosMeRowCount() <= 0)
                {
                    Mensaje = Mensajes.MensajeSinDatosTabla;
                    MensajeCommand.Execute(null);
                    PopupShow = true;
                    await _navigation.PopModalAsync();
                    return;
                }

                if (findUserRemember is null)
                {
                    Mensaje = Mensajes.MensajeCredencialesInvalida;
                    MensajeCommand.Execute(null);
                    PopupShow = true;
                    await _navigation.PopModalAsync();
                    return;
                }

                VariablesGlobales.User = findUserRemember;
            }

            Current!.MainPage = new MainPage();
            await _navigation.PopModalAsync();
        }

        private void OnMensaje()
        {
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

        public override async void OnAppearing(INavigation navigation)
        {
            _navigation = navigation;
            var findUserRemember = await _repositoryTbUser!.PosmeFindUserRemember();
            if (findUserRemember is null) return;
            UserName = findUserRemember.Nickname!;
            Password = findUserRemember.Password!;
            Company = findUserRemember.Company!;
        }
    }
}