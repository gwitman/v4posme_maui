using CommunityToolkit.Maui.Views;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels;

public class DownloadViewModel : BaseViewModel
{
    private bool _popUpShow;
    private bool _switch;
    private string _mensaje;
    private Color _popupBackgroundColor = Colors.White;
    private readonly RestApiAppMobileApi _restApiDownload;
    private readonly HelperContador _helperContador;

    public DownloadViewModel()
    {
        _helperContador = VariablesGlobales.UnityContainer.Resolve<HelperContador>();
        _restApiDownload = new RestApiAppMobileApi();
        _mensaje = string.Empty;
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
        set => SetValue(ref _popupBackgroundColor, value, () => RaisePropertyChanged(nameof(PopupBackgroundColor)));
    }

    public string Mensaje
    {
        get => _mensaje;
        set => SetValue(ref _mensaje, value, () => RaisePropertyChanged(nameof(Mensaje)));
    }

    public bool PopUpShow
    {
        get => _popUpShow;
        set => SetValue(ref _popUpShow, value, () => RaisePropertyChanged(nameof(PopUpShow)));
    }

    public bool Switch
    {
        get => _switch;
        set => SetValue(ref _switch, value, () => RaisePropertyChanged(nameof(Switch)));
    }

    public Command DownloadCommand { get; }
}