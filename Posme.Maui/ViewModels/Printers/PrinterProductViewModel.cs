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
        var parametroPrinter = await _parameterSystem.PosMeFindPrinter();
        if (string.IsNullOrWhiteSpace(parametroPrinter.Value))
        {
            return;
        }

        var item = VariablesGlobales.Item;
        var printer = new Printer(parametroPrinter.Value);
        /*var barCodeImage = new BarcodeGeneratorView
        {
            Format = BarcodeFormat.Codabar,
            Value = item.BarCode,
            ForegroundColor = Colors.Black,
            HeightRequest = 100,
            WidthRequest = 100
        };
        var imageBarcode = await barCodeImage.CaptureAsync();
        var openReadAsync = await imageBarcode.OpenReadAsync();
        var skBitmap = SKBitmap.Decode(openReadAsync);
        printer.Image(skBitmap);*/
        printer.Code39(item.BarCode);
        printer.Append(item.Name);
        printer.Append(item.BarCode);
        printer.Append(item.ItemNumber!);
        printer.Append(item.PrecioPublico.ToString("N2"));
        printer.NewLines(2);
        printer.Print();
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