using SkiaSharp;

namespace Posme.Maui.HelpersPrinters.Interfaces.Command
{
    internal interface IImage
    {
        byte[] Print(SKBitmap image);
    }
}
