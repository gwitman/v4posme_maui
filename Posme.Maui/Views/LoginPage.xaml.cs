#nullable enable
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly RestServiceUser _restServiceUser = new();

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            var findUserRemember = await _restServiceUser.FindUserRemember();
            if (findUserRemember is null) return;
            ((LoginViewModel)BindingContext).UserName = findUserRemember.Nickname;
            ((LoginViewModel)BindingContext).Password = findUserRemember.Password;
        }

        private async void DXButtonBase_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage());
            var model = (LoginViewModel)BindingContext;
            VariablesGlobales.CompanyKey = model.Company.ToLower();
            model.PopupShow = await _restServiceUser.FindUserApi(model.UserName, model.Password);
            if (!model.PopupShow)
            {
                model.MensajeCommand.Execute(null);
                Popup.Show();
                await Navigation.PopModalAsync();
                return;
            }

            if (ChkRemember.IsChecked!.Value)
            {
                var findUserRemember = await _restServiceUser.FindUserByNicknameAndPassword(model.UserName, model.Password);
                await _restServiceUser.OnRemember();
                if (findUserRemember is not null)
                {
                    findUserRemember.Remember = true;
                    await _restServiceUser.UpdateUser(findUserRemember);
                }
                else
                {
                    VariablesGlobales.User.Remember = true;
                    await _restServiceUser.InsertUser(VariablesGlobales.User);
                }
            }
            else
            {
                await _restServiceUser.OnRemember();
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