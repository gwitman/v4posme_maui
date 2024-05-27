using System.Diagnostics;
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
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                var barCode = e.Results.FirstOrDefault();
                if (barCode is null)
                {
                    VariablesGlobales.BarCode = "";
                    return;
                }

                VariablesGlobales.BarCode = barCode.Value;
                if (Navigation.ModalStack.Count > 0)
                {
                    Navigation.PopModalAsync();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        });
    }
}