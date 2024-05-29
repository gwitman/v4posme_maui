using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views.Clientes;

public partial class ClientesPage : ContentPage
{
    private ClientesViewModel? _clientesViewModel;
    
    public ClientesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        _clientesViewModel = (ClientesViewModel)BindingContext;
        _clientesViewModel.OnAppearing(Navigation);
    }
}