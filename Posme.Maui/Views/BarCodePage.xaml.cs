using Posme.Maui.Services.Helpers;
using ZXing.Net.Maui;

namespace Posme.Maui.Views;

public partial class BarCodePage : ContentPage
{
    public BarCodePage()
    {
        InitializeComponent();
        BarcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        var barCode = e.Results.FirstOrDefault();
        if (barCode is null)
        {
          VariablesGlobales.BarCode = "";
            return;
        }

        VariablesGlobales.BarCode = barCode.Value;
        await Navigation.PopAsync(true);
    }
}