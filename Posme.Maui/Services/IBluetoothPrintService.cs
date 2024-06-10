namespace Posme.Maui.Services;

public interface IBluetoothPrintService
{
    void Connect(string deviceName);
    void Print(string text);
    void Disconnect();
}