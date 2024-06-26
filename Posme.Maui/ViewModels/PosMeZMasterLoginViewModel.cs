﻿using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;
using Posme.Maui.Services.Api;
using Posme.Maui.Services.SystemNames;
using static Microsoft.Maui.Controls.Application;

namespace Posme.Maui.ViewModels
{
    public class PosMeZMasterLoginViewModel : BaseViewModel
    {
        private readonly RestApiCoreAcount _restServiceUser = new();
        private readonly IRepositoryTbUser _repositoryTbUser;
        private string? _userName;
        private string? _password;
        private bool _opcionPagar;
        private string? _company;
        private bool _popupShow;
        private bool _remember;
        private readonly IRepositoryTbCompany _repositoryTbCompany;

        public PosMeZMasterLoginViewModel()
        {
            _repositoryTbUser = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbUser>();
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            MensajeCommand = new Command(OnMensaje, ValidateError);
            PropertyChanged += (_, _) => LoginCommand.ChangeCanExecute();
            _repositoryTbCompany = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCompany>();
            RealizarPagoCommand = new Command(OnRealizarPagoCommand);
        }

        private async void OnRealizarPagoCommand(object obj)
        {
            if (ValidateLogin())
            {
                return;
            }

            var findUserRemember =
                await _repositoryTbUser.PosMeFindUserByNicknameAndPassword(UserName!, Password!);
            if (findUserRemember is not null)
            {
                
            }
        }

        public Command LoginCommand { get; }
        private Command MensajeCommand { get; }


        public bool PopupShow
        {
            get => _popupShow;
            set => SetProperty(ref _popupShow, value);
        }

        public string? UserName
        {
            get => _userName;
            set => SetProperty(ref this._userName, value);
        }

        public string? Password
        {
            get => _password;
            set => SetProperty(ref this._password, value);
        }

        public string? Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        public bool Remember
        {
            get => _remember;
            set => SetProperty(ref _remember, value);
        }

        public bool OpcionPagar
        {
            get => _opcionPagar;
            set => SetProperty(ref this._opcionPagar, value);
        }

        public Command RealizarPagoCommand { get; }

        private async void OnLoginClicked()
        {
            await Navigation!.PushModalAsync(new LoadingPage());
            VariablesGlobales.CompanyKey = Company!.ToLower();
            var findUserRemember =
                await _repositoryTbUser.PosMeFindUserByNicknameAndPassword(UserName!, Password!);
            if (Remember)
            {
                var response = await _restServiceUser.LoginMobile(UserName!, Password!);
                if (!response)
                {
                    Mensaje = Mensajes.MensajeCredencialesInvalida;
                    PopupShow = true;
                    await Navigation.PopModalAsync();
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
                    await Navigation.PopModalAsync();
                    return;
                }

                if (findUserRemember is null)
                {
                    Mensaje = Mensajes.MensajeCredencialesInvalida;
                    MensajeCommand.Execute(null);
                    PopupShow = true;
                    await Navigation.PopModalAsync();
                    return;
                }

                VariablesGlobales.User = findUserRemember;
            }

            VariablesGlobales.TbCompany = await _repositoryTbCompany.PosMeFindFirst();
            Current!.MainPage = new MainPage();
            await Navigation.PopModalAsync();
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

        public async void OnAppearing(INavigation navigation)
        {
            Navigation = navigation;
            var findUserRemember = await _repositoryTbUser.PosmeFindUserRemember();
            if (findUserRemember is null) return;
            UserName = findUserRemember.Nickname!;
            Password = findUserRemember.Password!;
            Company = findUserRemember.Company!;
        }
    }
}