using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.ViewModels.Invoices;

namespace Posme.Maui.Views.Invoices;

public partial class PrinterInvoicePage : ContentPage
{
    public PrinterInvoicePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((PrinterInvoiceViewModel)BindingContext).OnAppearing(Navigation);
    }

    private void MenuItem_OnClicked(object? sender, EventArgs e)
    {
    }
}