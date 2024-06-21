using System.Collections.ObjectModel;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Unity;

namespace Posme.Maui.ViewModels.Invoices;

public class PrinterInvoiceViewModel : BaseViewModel
{
    private readonly IRepositoryTbParameterSystem _parameterSystem;

    public PrinterInvoiceViewModel()
    {
        Title = "Comprobante Pago Factura";
        _parameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        AplicarOtroCommand = new Command(OnAplicarOtroCommand);
        PrintCommand = new Command(OnPrintCommand);
    }

    private void OnPrintCommand(object obj)
    {
    }

    private void OnAplicarOtroCommand(object obj)
    {
        var stack = Shell.Current.Navigation.NavigationStack.ToArray();
        for (var i = stack.Length - 1; i > 0; i--)
        {
            Shell.Current.Navigation.RemovePage(stack[i]);
        }
    }

    private ImageSource? _logoSource;

    public ImageSource? LogoSource
    {
        get => _logoSource;
        set => SetProperty(ref _logoSource, value);
    }

    public Api_AppMobileApi_GetDataDownloadCustomerResponse CustomerResponse => VariablesGlobales.DtoInvoice.CustomerResponse!;
    public ViewTempDtoInvoice DtoInvoice => VariablesGlobales.DtoInvoice;
    public ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse> ItemsResponses => VariablesGlobales.DtoInvoice.Items;
    public string Moneda => VariablesGlobales.DtoInvoice.Currency!.Simbolo;
    public string Balance => VariablesGlobales.DtoInvoice.Balance.ToString("N2");
    public string Monto => VariablesGlobales.DtoInvoice.Monto.ToString("N2");
    public string Cambio => VariablesGlobales.DtoInvoice.Cambio.ToString("N2");
    public Command PrintCommand { get; }
    public Command AplicarOtroCommand { get; }


    public async void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        await Task.Run(async () =>
        {
            var paramter = await _parameterSystem.PosMeFindLogo();
            var imageBytes = Convert.FromBase64String(paramter.Value!);
            LogoSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        });
    }
}