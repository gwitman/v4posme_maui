using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.ViewModels.Printers;

namespace Posme.Maui.Views.Printers;

public partial class DashboardPrinterPage : ContentPage
{
    private readonly DashboardPrinterViewModel _viewModel;

    public DashboardPrinterPage()
    {
        InitializeComponent();
        _viewModel = (DashboardPrinterViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing(Navigation);
        _viewModel.Load();
    }
}