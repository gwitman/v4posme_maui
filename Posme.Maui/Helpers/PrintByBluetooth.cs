#if ANDROID
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Android.Bluetooth;
using ESC_POS_USB_NET.Printer;
using Java.Util;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;
using Image = System.Drawing.Image;

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
                if (string.IsNullOrWhiteSpace(logo.Value))
                {
                    var logoByte = Convert.FromBase64String(logo.Value!);
                    printer.AlignCenter();
                    printer.Image(logoByte);
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