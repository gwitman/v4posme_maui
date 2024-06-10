using Posme.Maui.Services.Helpers;

namespace Posme.Maui.ViewModels.Abonos;

public class PrintViewViewModel :BaseViewModel
{
    public PrintViewViewModel()
    {
        Title = "Imprimir Archivo";
    }

    public void OnAppearing()
    {
        DocumentStream = new FileStream(VariablesGlobales.FilePdf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }
    
    private Stream? _documentStrem;

    public Stream? DocumentStream
    {
        get => _documentStrem;
        set => SetProperty(ref _documentStrem, value);
    }
}