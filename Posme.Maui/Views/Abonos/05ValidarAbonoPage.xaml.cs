using System.Diagnostics;
using CommunityToolkit.Maui.Core;
using ESC_POS_USB_NET.Printer;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.ViewModels.Abonos;
using Unity;

namespace Posme.Maui.Views.Abonos;

public partial class ValidarAbonoPage : ContentPage
{
    private BluetoothPrinterService _bluetoothPrinterService;
    public ValidarAbonoPage()
    {
        InitializeComponent();
        _bluetoothPrinterService = new BluetoothPrinterService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ValidarAbonoViewModel)BindingContext).OnAppearing();
        Logo.Source = ((ValidarAbonoViewModel)BindingContext).LogoSource;
    }

    private async void MenuItem_OnClicked(object? sender, EventArgs e)
    {
        try
        {
            var filePath = await FileImage();
            await ShareImageAsync(filePath);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }

    private string GetFilePath(string filename)
    {
        string folderPath;

        folderPath = Environment.GetFolderPath(DeviceInfo.Platform == DevicePlatform.Android
            ? Environment.SpecialFolder.LocalApplicationData
            : Environment.SpecialFolder.MyDocuments);

        return Path.Combine(folderPath, filename);
    }

    private async Task ShareImageAsync(string imagePath)
    {
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Compartir Comprobante de abono",
            File = new ShareFile(imagePath)
        });
    }

    private async Task<string> FileImage()
    {
        var screenshotResult = await DxStackLayout.CaptureAsync();
        if (screenshotResult is null)
        {
            ((ValidarAbonoViewModel)BindingContext)
                .ShowToast("No fue posible realizar la captura de los datos para compartir",
                    ToastDuration.Long, 18);
            return "";
        }

        await using var stream = await screenshotResult.OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var dateTime = DateTime.Now;
        var result = $"{dateTime.Year}{dateTime.Month}{dateTime.Day}{dateTime.Hour}{dateTime.Minute}{dateTime.Second}";
        var filePath = GetFilePath($"{result}.png");
        await File.WriteAllBytesAsync(filePath, memoryStream.ToArray());
        return filePath;
    }

    private async void ItemPrint_OnClicked(object? sender, EventArgs e)
    {
        var isPrinterConnected = await _bluetoothPrinterService.IsPrinterConnectedAsync();

        if (!isPrinterConnected) {
            await DisplayAlert("Impresora", "No hay impresora conectada.", "OK");
            return;
        }
        
        await _bluetoothPrinterService.CheckAndRequestBluetoothPermissionsAsync();
        
        var printer = new Printer(_bluetoothPrinterService.PrinterName);
        printer.TestPrinter();
        printer.FullPaperCut();
        printer.PrintDocument();
    }
   

}