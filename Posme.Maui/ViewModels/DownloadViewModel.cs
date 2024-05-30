using Posme.Maui.Services.Helpers;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels;

public class DownloadViewModel : BaseViewModel
{
    private bool _popUpShow;
    private bool _switch;
    private Color _popupBackgroundColor = Colors.White;
    private readonly RestApiAppMobileApi _restApiDownload;
    private readonly Helper _helperContador;

    public DownloadViewModel()
    {
        _helperContador = VariablesGlobales.UnityContainer.Resolve<Helper>();
        _restApiDownload = new RestApiAppMobileApi();
        DownloadCommand = new Command(OnDownloadClicked, ValidateDownload);
        PropertyChanged += (_, _) => DownloadCommand.ChangeCanExecute();
    }

    private bool ValidateDownload()
    {
        return Switch;
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

    public Color PopupBackgroundColor
    {
        get => _popupBackgroundColor;
        set => SetProperty(ref _popupBackgroundColor, value);
    }

    public bool PopUpShow
    {
        get => _popUpShow;
        set => SetProperty(ref _popUpShow, value);
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