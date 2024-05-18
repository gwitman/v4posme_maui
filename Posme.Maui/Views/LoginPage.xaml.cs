#nullable enable
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private async void DXButtonBase_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage());
            var model = (LoginViewModel)BindingContext;
            VariablesGlobales.CompanyKey = model.Company.ToLower();
            var restServiceUser = new RestServiceUser();
            var validateCount = await restServiceUser.RowCount();
            ObjUser? findUserRemember = null;
            if (validateCount > 0)
            {
                findUserRemember = await restServiceUser.FindUserRemember();
            }

            if (findUserRemember is not null)
            {
                model.UserName = findUserRemember.Nickname;
                model.Password = findUserRemember.Password;
            }

            model.PopupShow = await restServiceUser.FindUser(model.UserName, model.Password);
            if (!model.PopupShow)
            {
                model.MensajeCommand.Execute(null);
                Popup.Show();
                return;
            }

            if (ChkRemember.IsChecked!.Value)
            {
                await restServiceUser.OnRemember();
                VariablesGlobales.User.Remember = true;
                await restServiceUser.InsertUser(VariablesGlobales.User);
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