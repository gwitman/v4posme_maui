using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.ViewModels;
using Posme.Maui.ViewModels.Abonos;
using Unity;

namespace Posme.Maui.Views.Abonos;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomerDetailInvoicePage : ContentPage
{
    private readonly CustomerDetailInvoiceViewModel _viewModel;

    public CustomerDetailInvoicePage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new CustomerDetailInvoiceViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing(Navigation);
    }
    }