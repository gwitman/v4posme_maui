using Posme.Maui.ViewModels;

namespace Posme.Maui.Views.Customers;

public partial class CustomersPage : ContentPage
{
    private PosMeCustomerViewModel? _clientesViewModel;
    
    public CustomersPage()
    {
        InitializeComponent();
        Title = "Clientes";
    }

    protected override void OnAppearing()
    {
        ((PosMeCustomerViewModel)BindingContext).OnAppearing(Navigation);
    }
}