using System.Diagnostics;
using DevExpress.Maui.Core;
using Posme.Maui.Services.Helpers;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((ItemsViewModel)BindingContext).OnAppearing(Navigation);
        }

        private async void SimpleButton_OnClicked(object? sender, EventArgs e)
        {
            var barCodePage = new BarCodePage();
            await Navigation.PushAsync(barCodePage);
            if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
            SearchBar.Text = VariablesGlobales.BarCode;
            VariablesGlobales.BarCode = "";
        }
    }
}