using Posme.Maui.HelpersPrinters.Extensions;
using Posme.Maui.HelpersPrinters.Interfaces.Command;

namespace Posme.Maui.Services.HelpersPrinters.Epson_Commands
{
    public class FontWidth : IFontWidth
    {
        public byte[] Normal()
        {
            return new byte[] { 27, '!'.ToByte(), 0 };
        }

        public byte[] DoubleWidth2()
        {
            return new byte[] { 29, '!'.ToByte(), 16 };
        }

        public byte[] DoubleWidth3()
        {
            return new byte[] { 29, '!'.ToByte(), 32 };
        }
    }
}

