#nullable enable
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.ViewModels;
using Posme.Maui.Services.Repository;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly RestApiCoreAcountMLogin _restServiceUser = new();
        private readonly TbUser _tbUserRespository = new();

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            var findUserRemember = await _tbUserRespository.PosmeFindUserRemember();
            if (findUserRemember is null) return;
            ((LoginViewModel)BindingContext).UserName = findUserRemember.Nickname;
            ((LoginViewModel)BindingContext).Password = findUserRemember.Password;
        }

        private async void DXButtonBase_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage());
            var model = (LoginViewModel)BindingContext;
            VariablesGlobales.CompanyKey = model.Company.ToLower();
            model.PopupShow = await _restServiceUser.LoginMobile(model.UserName, model.Password);
            if (!model.PopupShow)
            {
                model.MensajeCommand.Execute(null);
                Popup.Show();
                await Navigation.PopModalAsync();
                return;
            }

            if (ChkRemember.IsChecked!.Value)
            {
                var findUserRemember = await _tbUserRespository.PosMeFindUserByNicknameAndPassword(model.UserName, model.Password);
                await _tbUserRespository.PosMeOnRemember();
                if (findUserRemember is not null)
                {
                    findUserRemember.Remember = true;
                    await _tbUserRespository.PosMeUpdate(findUserRemember);
                }
                else
                {
                    VariablesGlobales.User.Remember = true;
                    await _tbUserRespository.PosMeInsert(VariablesGlobales.User);
                }
            }
            else
            {
                await _tbUserRespository.PosMeOnRemember();
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