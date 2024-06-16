using Posme.Maui.HelpersPrinters.Enums;

namespace Posme.Maui.HelpersPrinters.Interfaces.Command
{
    internal interface IQrCode
    {
        byte[] Print(string qrData);
        byte[] Print(string qrData, QrCodeSize qrCodeSize);
    }
}

