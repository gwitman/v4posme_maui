using System.Diagnostics;
using System.Text;
using CommunityToolkit.Maui.Core;
using ESC_POS_USB_NET.Printer;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.ViewModels.Abonos;
using Unity;

namespace Posme.Maui.Views.Abonos;

public partial class ValidarAbonoPage : ContentPage
{
    private PrinterServices _printerServices;
    //IBluetoothLE ble;
    //IAdapter adapter;
    //IDevice printerDevice;
    public ValidarAbonoPage()
    {
        InitializeComponent();
        _printerServices = new PrinterServices();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ValidarAbonoViewModel)BindingContext).OnAppearing(Navigation);
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
   
}