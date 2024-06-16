namespace Posme.Maui.HelpersPrinters.Interfaces.Command
{
    internal interface IPaperCut
    {
        byte[] Full();
        byte[] Partial();
    }
}

