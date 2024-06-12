#if ANDROID
using System.Text;
using Android.Bluetooth;
using ESC_POS_USB_NET.Printer;
using Java.Util;

namespace Posme.Maui;

public class PrintByBluetooth
{
    private BluetoothDevice? _device;
    private BluetoothSocket? _socket;

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
            /*var outputStream = _socket.OutputStream;
            outputStream!.Write(_buffer, 0, _buffer.Length);
            outputStream.Flush();*/
            var printer = new Printer();
            printer.Append("Test Printer");
            printer.Print(_socket);
        }
    }

  
    public void Disconnect()
    {
        _socket!.Close();
    }

}
#endif