namespace Posme.Maui.HelpersPrinters.Interfaces.Command
{
    internal interface IAlignment
    {
        byte[] Left();
        byte[] Right();
        byte[] Center();
    }
}

