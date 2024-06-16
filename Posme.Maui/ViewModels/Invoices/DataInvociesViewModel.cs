using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using Android.Media;
using CommunityToolkit.Maui.Core;
using DevExpress.Maui.DataGrid;
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
        Item = VariablesGlobales.DtoInvoice;
        SeleccionarProductosCommand = new Command(OnSeleccionarProductos, ValidateFields);
        PropertyChanged += (_, _) => SeleccionarProductosCommand.ChangeCanExecute();
    }

    private bool Validate()
    {
        return string.IsNullOrWhiteSpace(Comentarios) || string.IsNullOrWhiteSpace(Referencias);
    }


    private bool ValidateFields()
    {
        return !Validate();
    }

    private async void OnSeleccionarProductos()
    {
        if (!ValidateFields())
        {
            return;
        }

        if (SelectedCurrency is null)
        {
            ShowToast("Seleccione una moneda para continuar", ToastDuration.Long, 16);
            return;
        }

        if (SelectedTipoDocumento is null)
        {
            ShowToast("Seleccione un tipo de documento para continuar", ToastDuration.Long, 16);
            return;
        }

        Item.Comentarios = Comentarios;
        Item.Referencia = Referencias;
        Item.Currency = SelectedCurrency;
        Item.TipoDocumento = SelectedTipoDocumento;
        await NavigationService.NavigateToAsync<SeleccionarProductoViewModel>();
    }

    public bool ErrorCurrency { get; set; }
    public bool ErrorTipoDocumento { get; set; }
    public bool ErrorComentarios { get; set; }
    public bool ErrorReferencia { get; set; }
    public DtoInvoice Item { get; private set; }
    private string _comentarios;

    public string Comentarios
    {
        get => _comentarios;
        set => SetProperty(ref _comentarios, value);
    }

    private string _referencias;

    public string Referencias
    {
        get => _referencias;
        set => SetProperty(ref _referencias, value);
    }

    public ObservableCollection<DtoCurrency> Currencies { get; }
    public ObservableCollection<DtoTipoDocumento> TipoDocumentos { get; }
    public Command SeleccionarProductosCommand { get; }
    public DtoCurrency? SelectedCurrency { get; set;  }
    public DtoTipoDocumento? SelectedTipoDocumento { get; set; }

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
        Item = VariablesGlobales.DtoInvoice;
        VariablesGlobales.DtoInvoice.CustomerResponse = customer;
        Currencies.Clear();
        Currencies.Add(new DtoCurrency(1, "Córdobas"));
        Currencies.Add(new DtoCurrency(2, "Dolares"));
        TipoDocumentos.Clear();
        TipoDocumentos.Add(new DtoTipoDocumento(1, "Crédito"));
        TipoDocumentos.Add(new DtoTipoDocumento(2, "Contado"));
    }
}