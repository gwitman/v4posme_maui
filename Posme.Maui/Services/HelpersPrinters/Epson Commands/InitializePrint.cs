using Posme.Maui.HelpersPrinters.Extensions;
using Posme.Maui.HelpersPrinters.Interfaces.Command;

namespace Posme.Maui.Services.HelpersPrinters.Epson_Commands
{
    public class InitializePrint : IInitializePrint
    {
        public byte[] Initialize()
        {
            return new byte[] { 27, '@'.ToByte() };
        }
    }
}

