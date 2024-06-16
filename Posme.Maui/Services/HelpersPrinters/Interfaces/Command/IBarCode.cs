using Posme.Maui.Services.HelpersPrinters.Enums;

namespace Posme.Maui.Services.HelpersPrinters.Interfaces.Command
{
    interface IBarCode
    {
        byte[] Code128(string code,Positions printString);
        byte[] Code39(string code, Positions printString);
        byte[] Ean13(string code, Positions printString);
    }
}

