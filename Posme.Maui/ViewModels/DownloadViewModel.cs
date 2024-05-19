using Posme.Maui.Services.Helpers;
using Posme.Maui.Views;
namespace Posme.Maui.ViewModels;

public class DownloadViewModel : BaseViewModel
{
    private bool _popUpShow;
    private bool _switch;
    private string _mensaje;
    private Color _popupBackgroundColor = Colors.White; 
    
    public DownloadViewModel()
    {
        DownloadCommand = new Command(OnDownloadClicked, ValidateDownload);
        PropertyChanged += (_, _) => DownloadCommand.ChangeCanExecute();
    }

    private bool ValidateDownload()
    {
        return Switch;
    }

    private async void OnDownloadClicked()
    {
        
    }

    public Color PopupBackgroundColor
    {
        get => _popupBackgroundColor;
        set => SetProperty(ref _popupBackgroundColor, value);
    }

    public string Mensaje
    {
        get => _mensaje;
        set => SetProperty(ref _mensaje, value);
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
}