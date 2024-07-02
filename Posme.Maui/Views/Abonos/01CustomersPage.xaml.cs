using Posme.Maui.ViewModels.Abonos;

namespace Posme.Maui.Views.Abonos;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AbonosPage : ContentPage
{
    private readonly AbonosViewModel _viewModel;

    public AbonosPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new AbonosViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing(Navigation);
        await _viewModel.LoadsClientes();
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }
}