using System.Diagnostics;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new ItemsViewModel();
        }

       ItemsViewModel ViewModel { get; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing(Navigation);
        }

        private void SearchBar_OnTextChanged(object? sender, EventArgs eventArgs)
        {
            var textEdit = (DevExpress.Maui.Editors.TextEdit)sender!;
            /*if (string.IsNullOrEmpty(searchBar.Text))
            {
                ViewModel.LoadItemsCommand.Execute(null);
            }*/
        }
    }
}