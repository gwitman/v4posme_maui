using Posme.Maui.ViewModels.Abonos;

namespace Posme.Maui.Views.Abonos;

public partial class ValidarAbonoPage : ContentPage
{
    public ValidarAbonoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((ValidarAbonoViewModel)BindingContext).OnAppearing();
    }
}