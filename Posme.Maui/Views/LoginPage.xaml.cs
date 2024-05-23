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
            var model = (LoginViewModel)BindingContext;
            VariablesGlobales.CompanyKey = model.Company.ToLower();
            var findUserRemember =
                 await _tbUserRespository!.PosMeFindUserByNicknameAndPassword(model.UserName, model.Password);
            if (ChkRemember.IsChecked!.Value)
            {
                model.PopupShow = await _restServiceUser.LoginMobile(model.UserName, model.Password);
                if (!model.PopupShow)
                {
                    model.Mensaje = model.PopupShow ? "" : Mensajes.MensajeCredencialesInvalida;
                    model.MensajeCommand.Execute(null);
                    Popup.Show();
                    await Navigation.PopModalAsync();
                    return;
                }

                await _tbUserRespository.PosMeOnRemember();
                if (findUserRemember is not null)
                {
                    findUserRemember.Remember = true;
                    findUserRemember.Company = model.Company;
                    await _tbUserRespository.PosMeUpdate(findUserRemember);
                }
                else
                {
                    VariablesGlobales.User!.Company = model.Company;
                    VariablesGlobales.User.Remember = true;
                    await _tbUserRespository.PosMeInsert(VariablesGlobales.User);
                }
            }
            else
            {
                if (await _tbUserRespository.PosMeRowCount() <= 0)
                {
                    model.Mensaje = Mensajes.MensajeSinDatosTabla;
                    model.MensajeCommand.Execute(null);
                    Popup.Show();
                    await Navigation.PopModalAsync();
                    return;
                }
                if (findUserRemember is null)
                {
                    model.Mensaje = Mensajes.MensajeCredencialesInvalida;
                    model.MensajeCommand.Execute(null);
                    Popup.Show();
                    await Navigation.PopModalAsync();
                    return;
                }

                VariablesGlobales.User = findUserRemember;
            }

            await Navigation.PopModalAsync();
            Application.Current!.MainPage = new MainPage();
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}