using Posme.Maui.ViewModels;

namespace Posme.Maui.Views.Customers;

public partial class CustomersPage : ContentPage
{
    private CustomerViewModel? _clientesViewModel;
    
    public CustomersPage()
    {
        InitializeComponent();
        Title = "Clientes";
    }

    protected override void OnAppearing()
    {
        _clientesViewModel = (CustomerViewModel)BindingContext;
        _clientesViewModel.OnAppearing(Navigation);
    }
}