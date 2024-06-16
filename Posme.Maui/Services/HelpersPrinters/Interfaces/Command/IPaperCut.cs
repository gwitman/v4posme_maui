namespace Posme.Maui.Services.HelpersPrinters.Interfaces.Command
{
    internal interface IPaperCut
    {
        byte[] Full();
        byte[] Partial();
    }
}

