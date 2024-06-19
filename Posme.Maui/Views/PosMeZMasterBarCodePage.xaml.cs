using Posme.Maui.Services.SystemNames;
using ZXing.Net.Maui;

namespace Posme.Maui.Views;

public partial class BarCodePage : ContentPage
{
    private bool _isAnimating = true;
    private const int StepSize = 100;
    private double _currentY = 0;

    public BarCodePage()
    {
        InitializeComponent();
        BarcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = false
        };
        StartScanAnimation();
    }

    private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        await Task.Run(() =>
        {
            var barCode = e.Results.FirstOrDefault();
            if (barCode is null)
            {
                VariablesGlobales.BarCode = "";
                return;
            }

            BarCode = barCode.Value;
            VariablesGlobales.BarCode = barCode.Value;
            if (Navigation.ModalStack.Count <= 0) return;
            StopScanAnimation();
            Navigation.PopModalAsync();
        });
    }

    private async void StartScanAnimation()
    {
        _isAnimating = true;

        while (_isAnimating)
        {
            _currentY += StepSize;
            if (_currentY >= Height)
            {
                _currentY = 0;
            }

            await ScanLine.TranslateTo(0, _currentY, 250, Easing.CubicOut);
        }
    }

    private void StopScanAnimation()
    {
        _isAnimating = false;
        ScanLine.AbortAnimation("TranslateTo");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopScanAnimation();
    }

    public string BarCode { get; private set; }
}