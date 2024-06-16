using Posme.Maui.Services.HelpersPrinters.Interfaces.Command;

namespace Posme.Maui.Services.HelpersPrinters.Epson_Commands
{
    public class Drawer : IDrawer
    {
        public byte[] Open()
        {
            return new byte[] { 27, 112, 0, 60, 120 };
        }
    }
}

