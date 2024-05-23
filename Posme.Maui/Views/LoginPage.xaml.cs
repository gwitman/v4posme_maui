#nullable enable
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.ViewModels;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IRepositoryTbUser? _tbUserRespository;
        private readonly RestApiCoreAcount _restServiceUser = new();
        private readonly LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();
            _tbUserRespository = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbUser>();
        }

        protected override async void OnAppearing()
        {
            var findUserRemember = await _tbUserRespository!.PosmeFindUserRemember();
            if (findUserRemember is null) return;
            _viewModel.UserName = findUserRemember.Nickname!;
            _viewModel.Password = findUserRemember.Password!;
            _viewModel.Company = findUserRemember.Company!;
        }

        private async void DXButtonBase_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage());
            VariablesGlobales.CompanyKey = _viewModel.Company.ToLower();
            var taskPrincipal= Task.Run(async () =>
            {
                var findUserRemember =
                    await _tbUserRespository!.PosMeFindUserByNicknameAndPassword(_viewModel.UserName, _viewModel.Password);
                if (ChkRemember.IsChecked!.Value)
                {
                    _viewModel.PopupShow = await _restServiceUser.LoginMobile(_viewModel.UserName, _viewModel.Password);
                    if (!_viewModel.PopupShow)
                    {
                        _viewModel.Mensaje = _viewModel.PopupShow ? "" : Mensajes.MensajeCredencialesInvalida;
                        _viewModel.MensajeCommand.Execute(null);
                        Popup.Show();
                        await Navigation.PopModalAsync();
                        return;
                    }

                    await _tbUserRespository.PosMeOnRemember();
                    if (findUserRemember is not null)
                    {
                        findUserRemember.Remember = true;
                        findUserRemember.Company = _viewModel.Company;
                        await _tbUserRespository.PosMeUpdate(findUserRemember);
                    }
                    else
                    {
                        VariablesGlobales.User!.Company = _viewModel.Company;
                        VariablesGlobales.User.Remember = true;
                        await _tbUserRespository.PosMeInsert(VariablesGlobales.User);
                    }
                }
                else
                {
                    if (await _tbUserRespository.PosMeRowCount() <= 0)
                    {
                        _viewModel.Mensaje = Mensajes.MensajeSinDatosTabla;
                        _viewModel.MensajeCommand.Execute(null);
                        Popup.Show();
                        await Navigation.PopModalAsync();
                        return;
                    }
                    if (findUserRemember is null)
                    {
                        _viewModel.Mensaje = Mensajes.MensajeCredencialesInvalida;
                        _viewModel.MensajeCommand.Execute(null);
                        Popup.Show();
                        await Navigation.PopModalAsync();
                        return;
                    }

                    VariablesGlobales.User = findUserRemember;
                }
            });
            await taskPrincipal;
            Application.Current!.MainPage = new MainPage();
            await Navigation.PopModalAsync();
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}