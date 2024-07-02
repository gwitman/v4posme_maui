using CommunityToolkit.Maui.Core;
using Plugin.BLE;
using Posme.Maui.Models;
using Posme.Maui.Services.HelpersPrinters;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using SkiaSharp;
using Unity;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Posme.Maui.ViewModels.Printers;

public class PrinterProductViewModel : BaseViewModel
{
    private readonly IRepositoryTbParameterSystem _parameterSystem;
    private readonly IRepositoryParameters _repositoryParameters;

    public PrinterProductViewModel()
    {
        Title = "Detalle de Producto";
        _parameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        _repositoryParameters = VariablesGlobales.UnityContainer.Resolve<IRepositoryParameters>();
        PrinterCommand = new Command(OnPrinterCommand);
    }

    private async void OnPrinterCommand(object obj)
    {
        try
        {
            var parametroPrinter = await _parameterSystem.PosMeFindPrinter();
            if (string.IsNullOrWhiteSpace(parametroPrinter.Value))
            {
                return;
            }

            var item = VariablesGlobales.Item;
            var printer = new Printer(parametroPrinter.Value);
            if (!CrossBluetoothLE.Current.IsOn)
            {
                ShowToast(Mensajes.MensajeBluetoothState, ToastDuration.Long, 18);
                return;
            }

            if (printer.Device is null)
            {
                ShowToast(Mensajes.MensajeDispositivoNoConectado, ToastDuration.Long, 18);
            }
            printer.Code39CustomPosMe2px1p(item.BarCode);
            printer.Append(item.Name);
            printer.Append(item.BarCode);            
            printer.Append(item.PrecioPublico.ToString("N2"));
            printer.Append("-");
            printer.Avanza(35 /*8puntos = 1mm*/  );
            printer.Print();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private Api_AppMobileApi_GetDataDownloadItemsResponse _itemsResponse;

    public Api_AppMobileApi_GetDataDownloadItemsResponse ItemsResponse
    {
        get => _itemsResponse;
        set => SetProperty(ref _itemsResponse, value);
    }

    public Command PrinterCommand { get; }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        ItemsResponse = VariablesGlobales.Item;
    }
}