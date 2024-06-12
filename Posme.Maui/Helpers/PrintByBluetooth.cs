#if ANDROID
using System.Diagnostics;
using System.Text;
using Android.Bluetooth;
using Android.Graphics;
using ESC_POS_USB_NET.Printer;
using Java.Util;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui;

public class PrintByBluetooth
{
    private BluetoothDevice? _device;
    private BluetoothSocket? _socket;
    private IRepositoryTbParameterSystem ParameterSystem => VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();

    public void Connect(string deviceName)
    {
        var adapter = BluetoothAdapter.DefaultAdapter;
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

    private static byte[] ConvertBitmapToPosFormat(Bitmap bitmap)
    {
        // Opcionalmente, puedes redimensionar el Bitmap si es necesario
        //Bitmap scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, 384, bitmap.Height, false);
        var scaledBitmap = bitmap;

        var width = scaledBitmap.Width;
        var height = scaledBitmap.Height;

        var data = new byte[(width / 8) * height + 8];

        data[0] = 0x1D;
        data[1] = 0x76;
        data[2] = 0x30;
        data[3] = 0x00;
        data[4] = (byte)(width / 8);
        data[5] = 0x00;
        data[6] = (byte)(height % 256);
        data[7] = (byte)(height / 256);

        var k = 8;
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j += 8)
            {
                byte b = 0;
                for (var n = 0; n < 8; n++)
                {
                    if (j + n < width)
                    {
                        var pixel = scaledBitmap.GetPixel(j + n, i);
                        var r = (pixel >> 16) & 0xff;
                        var g = (pixel >> 8) & 0xff00;
                        var b1 = pixel & 0xff;
                        var luminance = (r + g + b1) / 3;
                        if (luminance < 128) {
                            b |= (byte)(1 << (7 - n));
                        }
                    }
                }

                data[k++] = b;
            }
        }

        return data;
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
                    var bitmap = BitmapFactory.DecodeByteArray(logoByte, 0, logoByte.Length);
                    var posData = ConvertBitmapToPosFormat(bitmap!);
                    printer.AlignCenter();
                    var outputStream = _socket.OutputStream;
                    outputStream!.Write(posData, 0, posData.Length);
                }

                printer.AlignLeft();
                printer.Append($"Le informamos que: Nombre: {VariablesGlobales.DtoAplicarAbono.FirstName} {VariablesGlobales.DtoAplicarAbono.LastName} creó un código para abono de factura con los siguientes datos");
                printer.Append($"Código de abono: {VariablesGlobales.DtoAplicarAbono.CodigoAbono}");
                printer.Append($"N°. Cedula: {VariablesGlobales.DtoAplicarAbono.Identification}");
                printer.Append($"Fecha: {VariablesGlobales.DtoAplicarAbono.Fecha.ToString("yyyy-M-d")}");
                printer.Append($"Saldo inicial: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoInicial.ToString("N2")}");
                printer.Append($"Monto a aplicar: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.MontoAplicar.ToString("N2")}");
                printer.Append($"Saldo Final: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoFinal.ToString("N2")}");
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