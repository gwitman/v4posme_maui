using Posme.Maui.ViewModels;

namespace Posme.Maui.Views.Customers;

public partial class CustomersPage : ContentPage
{
    private readonly PosMeCustomerViewModel _clientesViewModel;

    public CustomersPage()
    {
        InitializeComponent();
        Title = "Clientes";
        _clientesViewModel = (PosMeCustomerViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        _clientesViewModel.OnAppearing(Navigation);
    }
}