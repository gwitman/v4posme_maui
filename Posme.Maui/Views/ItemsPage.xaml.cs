using System.Diagnostics;
using DevExpress.Maui.Core;
using Posme.Maui.Services.Helpers;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        private readonly ItemsViewModel _viewModel;
        public ItemsPage()
        {
            InitializeComponent();
            _viewModel = (ItemsViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing(Navigation);
        }
    }
}