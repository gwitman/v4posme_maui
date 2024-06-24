using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.ViewModels.Printers;

namespace Posme.Maui.Views.Printers;

public partial class DashboardPrinterPage : ContentPage
{
    public DashboardPrinterPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((DashboardPrinterViewModel)BindingContext).OnAppearing(Navigation);
    }
}