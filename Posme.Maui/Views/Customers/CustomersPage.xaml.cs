using Posme.Maui.ViewModels;

namespace Posme.Maui.Views.Customers;

public partial class CustomersPage : ContentPage
{
    private ClientesViewModel? _clientesViewModel;
    
    public CustomersPage()
    {
        InitializeComponent();
        Title = "Listado de Clientes";
    }

    protected override void OnAppearing()
    {
        _clientesViewModel = (ClientesViewModel)BindingContext;
        _clientesViewModel.OnAppearing(Navigation);
    }
}