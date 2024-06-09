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
    private BluetoothPrinterService _bluetoothPrinterService;
    //IBluetoothLE ble;
    //IAdapter adapter;
    //IDevice printerDevice;
    public ValidarAbonoPage()
    {
        InitializeComponent();
        _bluetoothPrinterService = new BluetoothPrinterService();


        //ble = CrossBluetoothLE.Current;
        //adapter = CrossBluetoothLE.Current.Adapter;
        //
        //if (!ble.IsOn)
        //{
        //    
        //    return;
        //}
        //
        //adapter.DeviceDiscovered += (s, a) =>
        //{
        //    
        //
        //    if(a is not null )
        //    Debug.WriteLine(a.Device.Name);
        //
        //    if (a is not null)
        //    Debug.WriteLine(a.Device.Id.ToString());
        //
        //    if (a.Device.Name == "MP-5801") // Reemplaza con el nombre de tu impresora
        //    {
        //        adapter.StopScanningForDevicesAsync();
        //        printerDevice = a.Device;
        //    
        //    
        //    }
        //
        //    //adapter.StopScanningForDevicesAsync();
        //    //printerDevice = a.Device;
        //
        //};
        //
        //adapter.StartScanningForDevicesAsync();



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

        //-wgonazlez-if (printerDevice == null)
        //-wgonazlez-{
        //-wgonazlez-    await DisplayAlert("No Conectado", "No se encontró ninguna impresora.", "OK");
        //-wgonazlez-    return;
        //-wgonazlez-}
        //-wgonazlez-
        //-wgonazlez-try
        //-wgonazlez-{
        //-wgonazlez-    await adapter.ConnectToDeviceAsync(printerDevice);
        //-wgonazlez-    var service = await printerDevice.GetServiceAsync(Guid.Parse("00001101-0000-1000-8000-00805F9B34FB")); // Servicio SPP
        //-wgonazlez-    var characteristic = await service.GetCharacteristicAsync(Guid.Parse("00001101-0000-1000-8000-00805F9B34FB"));
        //-wgonazlez-
        //-wgonazlez-    // Comandos ESC/POS para imprimir texto
        //-wgonazlez-    string printText = "Hola, mundo!\n";
        //-wgonazlez-    byte[] bytes = Encoding.UTF8.GetBytes(printText); // Usar UTF-8 para codificar el texto
        //-wgonazlez-    await characteristic.WriteAsync(bytes);
        //-wgonazlez-
        //-wgonazlez-    await DisplayAlert("Impresión", "El texto se ha impreso correctamente.", "OK");
        //-wgonazlez-}
        //-wgonazlez-catch (Exception ex)
        //-wgonazlez-{
        //-wgonazlez-    await DisplayAlert("Error", $"No se pudo imprimir: {ex.Message}", "OK");
        //-wgonazlez-}



        var isPrinterConnected = await _bluetoothPrinterService.IsPrinterConnectedAsync();        
        if (!isPrinterConnected) {
            await DisplayAlert("Impresora", "No hay impresora conectada.", "OK");
            return;
        }
        
        //await _bluetoothPrinterService.CheckAndRequestBluetoothPermissionsAsync();
        var printer = new Printer(_bluetoothPrinterService.PrinterName);

        //try
        //{
        //    Encoding encoding = Encoding.UTF8;
        //    var printer = new Printer("InnerPrinter" );
        //    printer.TestPrinter();
        //    printer.FullPaperCut();
        //    printer.PrintDocument();
        //}
        //catch (Exception ex)
        //{
        //    var abc = "";
        //}
        //
        //try
        //{
        //    var printer = new Printer("Senraise_InnerPrinter","UTF8");
        //    printer.TestPrinter();
        //    printer.FullPaperCut();
        //    printer.PrintDocument();
        //}
        //catch (Exception ex)
        //{
        //    var adc = "";
        //}


    }


}