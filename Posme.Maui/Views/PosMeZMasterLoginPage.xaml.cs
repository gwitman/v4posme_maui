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
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ((PosMeZMasterLoginViewModel)BindingContext).OnAppearing(Navigation);
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}