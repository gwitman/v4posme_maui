using System.Collections.ObjectModel;
using System.Web;
using Android.Media;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.ViewModels.Invoices;

public class DataInvoicesViewModel : BaseViewModel, IQueryAttributable
{
    private IRepositoryTbCustomer _repositoryTbCustomer;

    public DataInvoicesViewModel()
    {
        _repositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        Title = "Datos de facturacion 2/5";
        Currencies = new();
        TipoDocumentos = new();
        Item = new DtoInvoice();
    }

    public DtoInvoice Item { get; set; }

    public ObservableCollection<DtoCurrency> Currencies { get; }
    public ObservableCollection<DtoTipoDocumento> TipoDocumentos { get; }

    public void OnAppearing(INavigation? navigation)
    {
        Navigation = navigation;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
        await LoadData(id);
    }

    private async Task LoadData(string? id)
    {
        var customer = await _repositoryTbCustomer.PosMeFindCustomer(id!);
        Item.CustomerNumber = id;
        Item.FirstName = customer.FirstName;
        Item.LastName = customer.LastName;
        Item.Balance = customer.Balance;
    }
}