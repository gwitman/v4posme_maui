#if ANDROID
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using ESC_POS_USB_NET.Printer;
using Java.Util;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using SkiaSharp;
using Unity;

namespace Posme.Maui;

public class PrintByBluetooth
{
    private BluetoothDevice? _device;
    private BluetoothSocket? _socket;
    private static IRepositoryTbParameterSystem ParameterSystem => VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();

    public void Connect(string deviceName)
    {
        var applicationContext = Platform.CurrentActivity!.ApplicationContext!;
        var bluetoothManager = (BluetoothManager)applicationContext.GetSystemService(Context.BluetoothService)!;
        var adapter = bluetoothManager.Adapter;
        //var adapter = BluetoothAdapter.DefaultAdapter;
        if (adapter == null || !adapter.IsEnabled)
        {
            throw new Exception("Bluetooth no está disponible o no está habilitado.");
        }

        var bondedDevices = adapter.BondedDevices;
        _device = bondedDevices.FirstOrDefault(d => d.Name == deviceName);

        if (_device == null)
        {
            throw new Exception($"No se encontró ningún dispositivo con el nombre {deviceName}.");
        }

        _socket = _device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
        _socket!.Connect();
    }

    private byte[] ConvertirImagen(SKBitmap imagen)
    {
        var ancho = imagen.Width;
        var alto = imagen.Height;

        var bytes = new List<byte>
        {
            // Comando para iniciar la impresión de la imagen
            0x1B, // Carácter de escape
            0x2A, // Comando *
            // Modo 8-dot single-density (densidad simple de 8 puntos)
            0x00,
            // Ancho de la imagen (bajo y alto)
            (byte)(ancho % 256),
            (byte)(ancho / 256)
        };

        for (var y = 0; y < alto; y++)
        {
            for (var x = 0; x < ancho; x++)
            {
                // Obtén el pixel en las coordenadas x, y
                var pixel = imagen.GetPixel(x, y);

                // Convierte el pixel a escala de grises
                var gris = (byte)(0.3 * pixel.Red + 0.59 * pixel.Green + 0.11 * pixel.Blue);

                // Convierte el pixel gris a blanco y negro
                var bw = (byte)(gris > 128 ? 0 : 1);

                // Añade el pixel a los bytes de la imagen
                bytes.Add(bw);
            }
        }

        return bytes.ToArray();
    }


    public void Print()
    {
        if (_socket is not null && _socket.IsConnected)
        {
            try
            {
                var logo = ParameterSystem.PosMeFindLogo().Result;
                /*var outputStream = _socket.OutputStream;
                outputStream!.Write(_buffer, 0, _buffer.Length);
                outputStream.Flush();*/
                var printer = new Printer();
                if (!string.IsNullOrWhiteSpace(logo.Value))
                {
                    var logoByte = Convert.FromBase64String(logo.Value!);
                    var bitmap = SKBitmap.Decode(logoByte);
                    printer.AlignCenter();
                    printer.Image(bitmap);
                    var outputStream = _socket.OutputStream;
                    var posData = ConvertirImagen(bitmap);
                    outputStream!.Write(posData, 0, posData.Length);
                }

                printer.AlignLeft();
                printer.Append($"Le informamos que: {VariablesGlobales.DtoAplicarAbono.FirstName} {VariablesGlobales.DtoAplicarAbono.LastName} creó un código para abono de factura con los siguientes datos");
                printer.Append($"Código de abono: {VariablesGlobales.DtoAplicarAbono.CodigoAbono}");
                printer.Append($"N°. Cedula: {VariablesGlobales.DtoAplicarAbono.Identification}");
                printer.Append($"Fecha: {VariablesGlobales.DtoAplicarAbono.Fecha:yyyy-M-d}");
                printer.Append($"Saldo inicial: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoInicial:N2}");
                printer.Append($"Monto a aplicar: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.MontoAplicar:N2}");
                printer.Append($"Saldo Final: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoFinal:N2}");
                printer.Append($"Comentarios: {VariablesGlobales.DtoAplicarAbono.Description}");
                printer.Print(_socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }


    public void Disconnect()
    {
        _socket!.Close();
    }
}
#endif