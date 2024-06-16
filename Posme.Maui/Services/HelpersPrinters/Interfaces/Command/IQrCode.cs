using Posme.Maui.Services.HelpersPrinters.Enums;

namespace Posme.Maui.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IQrCode
    {
        byte[] Print(string qrData);
        byte[] Print(string qrData, QrCodeSize qrCodeSize);
    }
}

