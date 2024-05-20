using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage(IServiceProvider services)
        {
            InitializeComponent();
            BindingContext = ViewModel = new ItemsViewModel(services);
        }

        ItemsViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private void SearchBar_OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if (string.IsNullOrEmpty(searchBar.Text))
            {
                ((ItemsViewModel)BindingContext).LoadItemsCommand.Execute(null);
            }
        }
    }
}