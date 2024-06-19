using System.Collections.ObjectModel;
using System.Diagnostics;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels.Invoices;

public class SeleccionarProductoViewModel : BaseViewModel
{
    private readonly IRepositoryItems _repositoryItems;

    public SeleccionarProductoViewModel()
    {
        Title = "Seleccionar producto 3/5";
        Productos = new();
        _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
        AnadirProducto = new Command<Api_AppMobileApi_GetDataDownloadItemsResponse>(OnAnadirProducto);
        SearchBarCodeCommand = new Command(OnSearchBarCode);
        SearchCommand = new Command(OnSearch);
        ProductosSeleccionadosCommand = new Command(OnRevisarProductos);
    }

    private async void OnRevisarProductos(object obj)
    {
        await NavigationService.NavigateToAsync<RevisarProductosSeleccionadosViewModel>();
    }

    private async void OnSearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            IsPanelVisible = !IsPanelVisible;
            return;
        }

        IsPanelVisible = !IsPanelVisible;
        IsBusy = true;
        await Task.Run(async () =>
        {
            Productos.Clear();
            var searchItems = await _repositoryItems.PosMeFilterdByItemNumberAndBarCodeAndName(Search);
            foreach (var itemsResponse in searchItems)
            {
                itemsResponse.MonedaSimbolo = VariablesGlobales.DtoInvoice.Currency!.Simbolo;
                Productos.Add(itemsResponse);
            }
        });
        IsBusy = false;
    }

    private async void OnSearchBarCode()
    {
        var barCodePage = new BarCodePage();
        await Navigation!.PushModalAsync(barCodePage);
        Search = barCodePage.BarCode;
        IsPanelVisible = !IsPanelVisible;
    }

    private void OnAnadirProducto(Api_AppMobileApi_GetDataDownloadItemsResponse? obj)
    {
        if (obj is null)
        {
            Debug.WriteLine("NO está pasando datos");
            return;
        }

        var cestaArticulos = VariablesGlobales.DtoInvoice.Items;
        var find = cestaArticulos.FirstOrDefault(response => response.ItemNumber == obj.ItemNumber);
        if (find is not null)
        {
            find.Quantity = decimal.Add(decimal.One, find.Quantity);
            find.Importe = decimal.Multiply(find.Quantity, find.PrecioPublico);
            VariablesGlobales.DtoInvoice.Balance += decimal.Multiply(decimal.One, find.PrecioPublico);
        }
        else
        {
            obj.Quantity = decimal.One;
            obj.Importe = decimal.Multiply(obj.Quantity, obj.PrecioPublico);
            VariablesGlobales.DtoInvoice.Items.Add(obj);
            VariablesGlobales.DtoInvoice.Balance += decimal.Multiply(decimal.One, obj.PrecioPublico);
        }

        VariablesGlobales.DtoInvoice.CantidadTotalSeleccionada ++;
        ProductosSeleccionadosCantidad = $"Enviar {VariablesGlobales.DtoInvoice.CantidadTotalSeleccionada} Items";
        ProductosSeleccionadosCantidadTotal = $"{VariablesGlobales.DtoInvoice.CantidadTotalSeleccionada} Items = {VariablesGlobales.DtoInvoice.Balance}";
    }

    private async void LoadProductos()
    {
        IsBusy = true;
        var findProductos = await _repositoryItems.PosMeTake10();
        Productos.Clear();
        foreach (var itemsResponse in findProductos)
        {
            itemsResponse.MonedaSimbolo = VariablesGlobales.DtoInvoice.Currency!.Simbolo;
            Productos.Add(itemsResponse);
        }

        IsBusy = false;
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        LoadProductos();
    }

    public ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse> Productos { get; }

    private string _productosSeleccionadosCantidadTotal = "Seleccione los productos";

    public string ProductosSeleccionadosCantidadTotal
    {
        get => _productosSeleccionadosCantidadTotal;
        set => SetProperty(ref _productosSeleccionadosCantidadTotal, value);
    }

    private string _productosSeleccionadosCantidad = "Seleccione los productos";

    public string ProductosSeleccionadosCantidad
    {
        get => _productosSeleccionadosCantidad;
        set => SetProperty(ref _productosSeleccionadosCantidad, value);
    }

    private int _cantidad;

    public int Cantidad
    {
        get => _cantidad;
        set => SetProperty(ref _cantidad, value);
    }

    public Command AnadirProducto { get; }
    public Command SearchCommand { get; }
    public Command SearchBarCodeCommand { get; }
    private bool _isPanelVisible;

    public bool IsPanelVisible
    {
        get => _isPanelVisible;
        set => SetProperty(ref _isPanelVisible, value);
    }

    public Command ProductosSeleccionadosCommand { get; }
}