using System.Diagnostics;
using System.Windows.Input;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.ViewModels;

public class ParameterViewModel : BaseViewModel
{
    private readonly IRepositoryTbParameterSystem _repositoryTbParameterSystem;
    private TbParameterSystem _posMeFindCounter = new();
    private TbParameterSystem _posMeFindLogo = new();
    private TbParameterSystem _posMeFindAccessPoint = new();
    public ICommand SaveCommand { get; }
    public ICommand FindImage { get; }

    public ParameterViewModel()
    {
        _repositoryTbParameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        Task.Run(async () =>
        {
            var test = await _repositoryTbParameterSystem.PosMeCount();
            Debug.WriteLine(test);
        });
        SaveCommand = new Command(OnSaveParameters);
        LoadValueContadorImagen();
    }

   private async void LoadValueContadorImagen()
    {
        _posMeFindCounter = await _repositoryTbParameterSystem.PosMeFindCounter();
        if (_posMeFindCounter != null)
        {
            Contador = Convert.ToInt32(_posMeFindCounter.Value);
        }

        _posMeFindLogo = await _repositoryTbParameterSystem.PosMeFindLogo();
        if (_posMeFindLogo != null && !string.IsNullOrWhiteSpace(_posMeFindLogo.Value))
        {
            var imageBytes = Convert.FromBase64String(_posMeFindLogo.Value!);
            ShowImage= ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        _posMeFindAccessPoint = await _repositoryTbParameterSystem.PosMeFindAccessPoint();
        if (_posMeFindAccessPoint != null)
        {
            PuntoAcceso = _posMeFindAccessPoint.Value;
        }
    }
    public bool Validate() {
        LogoHasError = string.IsNullOrWhiteSpace(Logo);
        PuntoAccesoHasError = string.IsNullOrWhiteSpace(PuntoAcceso);
        return !(LogoHasError || PuntoAccesoHasError );
    }
    public bool PuntoAccesoHasError { get; set; }

    public bool LogoHasError { get; set; }

    private void OnSaveParameters(object obj)
    {
        try
        {
            if (Validate())
            {
                _posMeFindLogo.Value = Logo;
                _repositoryTbParameterSystem.PosMeUpdate(_posMeFindLogo); 
                _posMeFindCounter.Value = Contador.ToString();
                _repositoryTbParameterSystem.PosMeUpdate(_posMeFindCounter);
                _repositoryTbParameterSystem.PosMeUpdate(_posMeFindAccessPoint);
                Mensaje = Mensajes.MensajeParametrosGuardar;
                PopupBackgroundColor = Colors.Green;
            }
            else
            {
                PopupBackgroundColor = Colors.Red;
                Mensaje = "Debe especificar los datos a guardar";
            }
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            PopupBackgroundColor = Colors.Red;
            Mensaje = ex.Message;
        }

        PopUpShow = true;
    }

    private int _contador;

    public int Contador
    {
        get => _contador;
        set => SetValue(ref _contador, value, () => RaisePropertyChanged(nameof(Contador)));
    }

    private string? _logo;

    public string? Logo
    {
        get => _logo;
        set => SetValue(ref _logo, value, () => RaisePropertyChanged(nameof(Logo)));
    }

    private string? _puntoAcceso;

    public string? PuntoAcceso
    {
        get => _puntoAcceso;
        set => SetValue(ref _puntoAcceso, value, () => RaisePropertyChanged(nameof(PuntoAcceso)));
    }

    private ImageSource _showImage;

    public ImageSource ShowImage
    {
        get => _showImage;
        set => SetValue(ref _showImage, value, () => RaisePropertyChanged(nameof(ShowImage)));
    }

    private bool _popUpShow;

    public bool PopUpShow
    {
        get => _popUpShow;
        set => SetValue(ref _popUpShow, value, () => RaisePropertyChanged(nameof(PopUpShow)));
    }

    private string? _mensaje;

    public string? Mensaje
    {
        get => _mensaje;
        set => SetValue(ref _mensaje, value, () => RaisePropertyChanged(nameof(Mensaje)));
    }
    private Color _popupBackgroundColor = Colors.White;
    public Color PopupBackgroundColor
    {
        get => _popupBackgroundColor;
        set => SetValue(ref _popupBackgroundColor, value, () => RaisePropertyChanged(nameof(PopupBackgroundColor)));
    }
}