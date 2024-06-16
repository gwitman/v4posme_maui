using Posme.Maui.HelpersPrinters.Interfaces.Command;

namespace Posme.Maui.HelpersPrinters.Epson_Commands
{
    public class Drawer : IDrawer
    {
        public byte[] Open()
        {
            return new byte[] { 27, 112, 0, 60, 120 };
        }
    }
}

