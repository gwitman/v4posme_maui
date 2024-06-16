using DevExpress.Maui.Core;
using DevExpress.Maui.DataForm;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Views.Items;

public partial class ItemEditPage : ContentPage
{
    DetailEditFormViewModel ViewModel => (DetailEditFormViewModel)BindingContext;
    private readonly IRepositoryItems _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
    private Api_AppMobileApi_GetDataDownloadItemsResponse _saveItem;
    private Api_AppMobileApi_GetDataDownloadItemsResponse _defaultItem;
    private readonly HelperCore _helperContador;

    public ItemEditPage()
    {
        InitializeComponent();
        _saveItem = new Api_AppMobileApi_GetDataDownloadItemsResponse();
        _defaultItem = new Api_AppMobileApi_GetDataDownloadItemsResponse();
        _helperContador = VariablesGlobales.UnityContainer.Resolve<HelperCore>();
        DataForm.CommitMode = CommitMode.Manually;
        Title = "Editar Producto";
    }

    private async void SaveItemClick(object sender, EventArgs e)
    {
        if (!DataForm.Validate())
        {
            TxtMensaje.Text = "Todos los campos son requeridos, intente nuevamente.";
            Popup.IsOpen = true;
            return;
        }

        DataForm.Commit();
        ViewModel.Save();
        _saveItem = (Api_AppMobileApi_GetDataDownloadItemsResponse)DataForm.DataObject;
        _saveItem.Modificado = true;
        if (ViewModel.IsNew)
        {
            await _repositoryItems.PosMeInsert(_saveItem);
        }
        else
        {
            await _repositoryItems.PosMeUpdate(_saveItem);
        }

        await _helperContador.PlusCounter();
    }

    private void DataForm_OnValidateForm(object sender, DataFormValidationEventArgs e)
    {
        _saveItem = (Api_AppMobileApi_GetDataDownloadItemsResponse)e.DataObject;
        if (string.IsNullOrWhiteSpace(_saveItem.ItemNumber))
        {
            e.HasErrors = true;
            TextItemNumber.HasError = true;
        }
        else
        {
            TextItemNumber.HasError = false;
        }

        if (string.IsNullOrWhiteSpace(_saveItem.BarCode))
        {
            e.HasErrors = true;
            TxtBarCode.HasError = true;
        }
        else
        {
            TxtBarCode.HasError = false;
        }

        if (string.IsNullOrWhiteSpace(_saveItem.Name))
        {
            e.HasErrors = true;
            TextName.HasError = true;
        }
        else
        {
            TextName.HasError = false;
        }

        if (string.IsNullOrWhiteSpace(TextPrecioPublico.Text))
        {
            e.HasErrors = true;
            TextPrecioPublico.HasError = true;
        }
        else
        {
            TextPrecioPublico.HasError = false;
        }
    }

    private async void SimpleButton_OnClicked(object? sender, EventArgs e)
    {
        var barCodePage = new BarCodePage();
        await Navigation.PushModalAsync(barCodePage);
        if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
        TxtBarCode.Text = VariablesGlobales.BarCode;
        VariablesGlobales.BarCode = "";
    }

    private void TextCantidadEntrada_OnTextChanged(object? sender, EventArgs e)
    {
        _saveItem = (Api_AppMobileApi_GetDataDownloadItemsResponse)DataForm.DataObject;
        _saveItem.CantidadFinal = decimal.Add(_saveItem.CantidadEntradas, _saveItem.Quantity) - _saveItem.CantidadSalidas;
        TextCantidadFinal.Text = _saveItem.CantidadFinal.ToString("N");
    }

    protected override async void OnAppearing()
    {
        _saveItem = (Api_AppMobileApi_GetDataDownloadItemsResponse)DataForm.DataObject;
        _defaultItem = await _repositoryItems.PosMeFindByItemNumber(_saveItem.ItemNumber);
        DataForm.CommitMode = CommitMode.LostFocus;
    }

    protected override void OnDisappearing()
    {
        if (!ViewModel.IsSaved)
        {
            TxtBarCode.Text = _defaultItem.BarCode;
            TextCantidadFinal.Text = _defaultItem.CantidadFinal.ToString("N");
            TextCantidadEntrada.Text = _defaultItem.CantidadEntradas.ToString("N");
            TextCantidadSalida.Text = _defaultItem.CantidadSalidas.ToString("N");
            TextName.Text = _defaultItem.Name;
            TextItemNumber.Text = _defaultItem.ItemNumber;
            TextPrecioPublico.Text = _defaultItem.PrecioPublico.ToString("N");
            DataForm.DataObject = _defaultItem;
        }
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }
}