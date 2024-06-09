﻿using CommunityToolkit.Maui.Core;
using DevExpress.Maui.Editors;
using Posme.Maui.ViewModels.Abonos;

namespace Posme.Maui.Views.Abonos;

public partial class AplicarAbonoPage : ContentPage
{

    public AplicarAbonoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((AplicarAbonoViewModel)BindingContext).OnAppearing(Navigation);
    }

    private void TxtMonto_OnTextChanged(object? sender, EventArgs e)
    {
        var text = sender as TextEdit;
        if (string.IsNullOrWhiteSpace(text!.Text))
        {
            ((AplicarAbonoViewModel)BindingContext).SaldoFinal = ((AplicarAbonoViewModel)BindingContext).SaldoInicial;
            TxtMonto.HasError = true;
        }
    }

    private void TxtDescription_OnTextChanged(object? sender, EventArgs e)
    {
        var text = sender as TextEdit;
        if (string.IsNullOrWhiteSpace(text!.Text))
        {
            TxtDescription.HasError = true;
        }
    }
}