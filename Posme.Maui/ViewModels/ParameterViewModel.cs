using System.Diagnostics;
using System.Windows.Input;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels;

public class ParameterViewModel : BaseViewModel
{
    private readonly IRepositoryTbParameterSystem _repositoryTbParameterSystem;
    private TbParameterSystem _posMeFindCounter = new();
    private TbParameterSystem _posMeFindLogo = new();
    private TbParameterSystem _posMeFindAccessPoint = new();
    private TbParameterSystem _posmeFindPrinter = new();
    public ICommand SaveCommand { get; }

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

    public override void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        LoadValueContadorImagen();
    }

    private async void LoadValueContadorImagen()
    {
        await Task.Run(async () =>
        {
            _posMeFindCounter = await _repositoryTbParameterSystem.PosMeFindCounter();
            if (!string.IsNullOrWhiteSpace(_posMeFindCounter.Value))
            {
                Contador = Convert.ToInt32(_posMeFindCounter.Value);
            }

            _posMeFindLogo = await _repositoryTbParameterSystem.PosMeFindLogo();
            if (!string.IsNullOrWhiteSpace(_posMeFindLogo.Value))
            {
                Logo = _posMeFindLogo.Value!;
                var imageBytes = Convert.FromBase64String(_posMeFindLogo.Value!);
                ShowImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }

            _posMeFindAccessPoint = await _repositoryTbParameterSystem.PosMeFindAccessPoint();
            if (!string.IsNullOrWhiteSpace(_posMeFindAccessPoint.Value))
            {
                PuntoAcceso = _posMeFindAccessPoint.Value;
            }

            _posmeFindPrinter = await _repositoryTbParameterSystem.PosMeFindPrinter();
            if (!string.IsNullOrWhiteSpace(_posmeFindPrinter.Value))
            {
                Printer = _posmeFindPrinter.Value;
            }
        });
    }

    private bool Validate()
    {
        LogoHasError = string.IsNullOrWhiteSpace(Logo);
        PuntoAccesoHasError = string.IsNullOrWhiteSpace(PuntoAcceso);
        PrinterHasError = string.IsNullOrWhiteSpace(Printer);
        return !(LogoHasError || PuntoAccesoHasError || PrinterHasError);
    }

    public bool PrinterHasError { get; set; }

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
                _posMeFindAccessPoint.Value = PuntoAcceso;
                _repositoryTbParameterSystem.PosMeUpdate(_posMeFindAccessPoint);
                _posmeFindPrinter.Value = Printer;
                _repositoryTbParameterSystem.PosMeUpdate(_posmeFindPrinter);
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

    private ImageSource? _showImage;

    public ImageSource? ShowImage
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

    private string? _printer;

    public string? Printer
    {
        get => _printer;
        set => SetValue(ref _printer, value, () => RaisePropertyChanged(nameof(Printer)));
    }
}