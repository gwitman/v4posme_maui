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
    private TbParameterSystem _posmeFindPrinter = new();
    public ICommand RefreshCommand { get; }
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
        RefreshCommand = new Command(OnRefreshPage);
        LoadValuesDefault();
    }

    private void OnRefreshPage(object obj)
    {
        LoadValuesDefault();
        IsRefreshing = false;
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        LoadValuesDefault();
    }

    private async void LoadValuesDefault()
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
    }

    private bool Validate()
    {
        PuntoAccesoHasError = string.IsNullOrWhiteSpace(PuntoAcceso);
        PrinterHasError = string.IsNullOrWhiteSpace(Printer);
        return !(PuntoAccesoHasError || PrinterHasError);
    }

    private bool _printerhasError;

    public bool PrinterHasError
    {
        get => _printerhasError;
        set => SetProperty(ref _printerhasError, value);
    }

    private bool _puntoAccesoHasError;

    public bool PuntoAccesoHasError
    {
        get => _puntoAccesoHasError;
        set => SetProperty(ref _puntoAccesoHasError, value);
    }

    private void OnSaveParameters(object obj)
    {
        try
        {
            if (Validate())
            {
                if (!string.IsNullOrWhiteSpace(VariablesGlobales.LogoTemp))
                {
                    _posMeFindLogo.Value = VariablesGlobales.LogoTemp;
                    _repositoryTbParameterSystem.PosMeUpdate(_posMeFindLogo);
                }

                _posMeFindCounter.Value = Contador.ToString();
                _repositoryTbParameterSystem.PosMeUpdate(_posMeFindCounter);
                _posMeFindAccessPoint.Value = PuntoAcceso;
                _repositoryTbParameterSystem.PosMeUpdate(_posMeFindAccessPoint);
                _posmeFindPrinter.Value = Printer;
                _repositoryTbParameterSystem.PosMeUpdate(_posmeFindPrinter);
                Mensaje = Mensajes.MensajeParametrosGuardar;
                PopupBackgroundColor = Colors.Green;
                LoadValuesDefault();
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
        set => SetProperty(ref _contador, value);
    }

    private string? _logo;

    public string? Logo
    {
        get => _logo;
        set => SetProperty(ref _logo, value);
    }

    private string? _puntoAcceso;

    public string? PuntoAcceso
    {
        get => _puntoAcceso;
        set => SetProperty(ref _puntoAcceso, value);
    }

    private ImageSource? _showImage;

    public ImageSource? ShowImage
    {
        get => _showImage;
        set
        {
            _showImage = value;
            SetProperty(ref _showImage, value);
        }
    }

    private bool _popUpShow;

    public bool PopUpShow
    {
        get => _popUpShow;
        set => SetProperty(ref _popUpShow, value);
    }

    private Color _popupBackgroundColor = Colors.Green;

    public Color PopupBackgroundColor
    {
        get => _popupBackgroundColor;
        set => SetProperty(ref _popupBackgroundColor, value);
    }

    private string? _printer;

    public string? Printer
    {
        get => _printer;
        set => SetProperty(ref _printer, value);
    }

    private bool _isRefreshing;

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }
}