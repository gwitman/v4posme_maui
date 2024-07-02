using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.ViewModels.Invoices;

namespace Posme.Maui.Views.Invoices;

public partial class InvoicesPage : ContentPage
{
    private InvoicesViewModel? _viewModel;
    public InvoicesPage()
    {
        InitializeComponent();
        _viewModel = (InvoicesViewModel?)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel!.OnAppearing(Navigation);
        _viewModel.LoadsClientes();
    }
}