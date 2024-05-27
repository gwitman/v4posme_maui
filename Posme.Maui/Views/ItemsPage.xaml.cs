using System.Diagnostics;
using DevExpress.Maui.Core;
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

        private void SearchBar_OnTextChanged(object? sender, EventArgs eventArgs)
        {
            
        }
    }
}