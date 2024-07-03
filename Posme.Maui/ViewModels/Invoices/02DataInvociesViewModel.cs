﻿using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using Android.Media;
using CommunityToolkit.Maui.Core;
using DevExpress.Maui.DataGrid;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Api;
using Unity;

namespace Posme.Maui.ViewModels.Invoices;

public class DataInvoicesViewModel : BaseViewModel, IQueryAttributable
{
    private IRepositoryTbCustomer _repositoryTbCustomer;

    public DataInvoicesViewModel()
    {
        _repositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        Title = "Datos de facturacion 2/5";
        Item = VariablesGlobales.DtoInvoice;
        SeleccionarProductosCommand = new Command(OnSeleccionarProductos, ValidateFields);
        PropertyChanged += (_, _) => SeleccionarProductosCommand.ChangeCanExecute();
        LoadComboBox();
    }

    private bool Validate()
    {
        return string.IsNullOrWhiteSpace(Comentarios);
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

        IsBusy = true;
        if (SelectedCurrency is null)
        {
            ShowToast(Mensajes.MensajeSeleccionarMoneda, ToastDuration.Long, 16);
            return;
        }

        if (SelectedTipoDocumento is null)
        {
            ShowToast(Mensajes.MensajeSeleccionarTipoDocumento, ToastDuration.Long, 16);
            return;
        }

        Item.Comentarios = Comentarios;
        Item.Referencia = Referencias;
        Item.Currency = SelectedCurrency;
        Item.TipoDocumento = SelectedTipoDocumento;
        await NavigationService.NavigateToAsync<SeleccionarProductoViewModel>();
        IsBusy = false;
    }

    public bool ErrorCurrency { get; set; }
    public bool ErrorTipoDocumento { get; set; }
    public bool ErrorComentarios { get; set; }
    public bool ErrorReferencia { get; set; }
    public ViewTempDtoInvoice Item { get; private set; }
    private string _comentarios="Sin Comentarios";

    public string Comentarios
    {
        get => _comentarios;
        set => SetProperty(ref _comentarios, value);
    }

    private string _referencias=string.Empty;

    public string Referencias
    {
        get => _referencias;
        set => SetProperty(ref _referencias, value);
    }

    private ObservableCollection<DtoCatalogItem>? _currencies;

    public ObservableCollection<DtoCatalogItem>? Currencies
    {
        get => _currencies;
        set => SetProperty(ref _currencies, value);
    }

    private ObservableCollection<DtoCatalogItem>? _tipoDocumentos;

    public ObservableCollection<DtoCatalogItem>? TipoDocumentos
    {
        get => _tipoDocumentos;
        set => SetProperty(ref _tipoDocumentos, value);
    }

    public Command SeleccionarProductosCommand { get; }
    public DtoCatalogItem? SelectedCurrency { get; set; }
    public DtoCatalogItem? SelectedTipoDocumento { get; set; }

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
        LoadComboBox();
        IsBusy = false;
    }

    private void LoadComboBox()
    {
        Currencies =
        [
            new DtoCatalogItem((int)TypeCurrency.Cordoba, "Córdobas", "C$"),
            new DtoCatalogItem((int)TypeCurrency.Dolar, "Dolares", "$")
        ];
        TipoDocumentos =
        [
            new DtoCatalogItem((int)TypeTransactionCausal.Contado, "Contado", "D"),
            new DtoCatalogItem((int)TypeTransactionCausal.Credito, "Crédito", "C")
        ];
        if (Currencies.Any())
        {
            SelectedCurrency = Currencies.First();
        }

        if (TipoDocumentos.Any())
        {
            SelectedTipoDocumento = TipoDocumentos.First();
        }
    }
}