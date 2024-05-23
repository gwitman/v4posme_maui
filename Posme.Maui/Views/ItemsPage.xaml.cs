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

        private void SearchBar_OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            var searchBar = (SearchBar)sender!;
            if (string.IsNullOrEmpty(searchBar.Text))
            {
                ViewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}