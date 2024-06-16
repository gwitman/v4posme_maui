using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
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
            VariablesGlobales.DtoInvoice.Balance = decimal.Multiply(find.Quantity, find.PrecioPublico);
        }
        else
        {
            obj.Quantity = decimal.One;
            VariablesGlobales.DtoInvoice.Items.Add(obj);
            VariablesGlobales.DtoInvoice.Balance = decimal.Multiply(obj.Quantity, obj.PrecioPublico);
        }

        Cantidad++;
        ProductosSeleccionados = $"{Cantidad} Items = {VariablesGlobales.DtoInvoice.Balance}";
    }

    private async void LoadProductos()
    {
        IsBusy = true;
        var findProductos = await _repositoryItems.PosMeTake10();
        Productos.Clear();
        foreach (var itemsResponse in findProductos)
        {
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

    private string _productosSeleccionados = "Seleccione los productos";

    public string ProductosSeleccionados
    {
        get => _productosSeleccionados;
        set => SetProperty(ref _productosSeleccionados, value);
    }

    private int _cantidad;

    public int Cantidad
    {
        get => _cantidad;
        set => SetProperty(ref _cantidad, value);
    }

    public Command AnadirProducto { get; }
}