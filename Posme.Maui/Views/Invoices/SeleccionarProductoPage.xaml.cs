using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.ViewModels.Invoices;

namespace Posme.Maui.Views.Invoices;

public partial class SeleccionarProductoPage : ContentPage
{
    public SeleccionarProductoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((SeleccionarProductoViewModel)BindingContext).OnAppearing(Navigation);
    }
}