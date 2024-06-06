using System.Web;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;

namespace Posme.Maui.ViewModels.Abonos;

public class ValidarAbonoViewModel : BaseViewModel, IQueryAttributable
{
    public ValidarAbonoViewModel()
    {
        Title = "Aplicar Abono 5/5";
        Item = VariablesGlobales.DtoAplicarAbono;
    }

    public DtoAbono Item { get; set; }

    public override async Task InitializeAsync(object parameter)
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
    }

    public void OnAppearing()
    {
        Item = VariablesGlobales.DtoAplicarAbono;
    }
}