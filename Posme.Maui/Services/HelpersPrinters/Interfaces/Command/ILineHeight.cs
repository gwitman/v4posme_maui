namespace Posme.Maui.HelpersPrinters.Interfaces.Command
{
    interface ILineHeight
    {
        byte[] Normal();
        byte[] SetLineHeight(byte height);
    }
}
