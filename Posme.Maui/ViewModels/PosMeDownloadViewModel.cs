using Posme.Maui.Services.Helpers;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels;

public class PosMeDownloadViewModel : BaseViewModel
{
    private bool _switch;
    private readonly RestApiAppMobileApi _restApiDownload;
    private readonly HelperCore _helperContador;

    public PosMeDownloadViewModel()
    {
        _helperContador = VariablesGlobales.UnityContainer.Resolve<HelperCore>();
        _restApiDownload = new RestApiAppMobileApi();
        DownloadCommand = new Command(OnDownloadClicked, ValidateDownload);
        PropertyChanged += (_, _) => DownloadCommand.ChangeCanExecute();
    }

    private bool ValidateDownload()
    {
        return Connectivity.Current.NetworkAccess != NetworkAccess.None && Switch;
    }

    private async void OnDownloadClicked()
    {
        await Navigation!.PushModalAsync(new LoadingPage());
        var counter = await _helperContador.GetCounter();
        if (counter != 0)
        {
            Mensaje = Mensajes.MensajeDownloadCantidadTransacciones;
            PopupBackgroundColor = Colors.Red;
            PopUpShow = true;
            await Navigation.PopModalAsync();
            return;
        }

        var result = await _restApiDownload.GetDataDownload();
        if (result)
        {
            PopupBackgroundColor = Colors.Green;
            Mensaje = Mensajes.MensajeDownloadSuccess;
        }
        else
        {
            Mensaje = Mensajes.MensajeDownloadError;
            PopupBackgroundColor = Colors.Red;
        }

        PopUpShow = true;
        await Navigation.PopModalAsync();
    }

    public bool Switch
    {
        get => _switch;
        set => SetProperty(ref _switch, value);
    }

    public Command DownloadCommand { get; }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
    }
}