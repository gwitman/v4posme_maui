using Posme.Maui.ViewModels.Abonos;

namespace Posme.Maui.Views.Abonos;

public partial class ValidarAbonoPage : ContentPage
{
    public ValidarAbonoPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        return base.OnBackButtonPressed();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ValidarAbonoViewModel)BindingContext).OnAppearing();
        Logo.Source = ((ValidarAbonoViewModel)BindingContext).LogoSource;
    }
}