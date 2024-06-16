using SkiaSharp;

namespace Posme.Maui.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IImage
    {
        byte[] Print(SKBitmap image);
    }
}
