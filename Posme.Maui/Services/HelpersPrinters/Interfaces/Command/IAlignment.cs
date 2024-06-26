namespace Posme.Maui.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IAlignment
    {
        byte[] Left();
        byte[] Right();
        byte[] Center();
        byte[] Avanza(int puntos);
    }
}

