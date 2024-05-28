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
    private AppMobileApiMGetDataDownloadItemsResponse _saveItem;
    private AppMobileApiMGetDataDownloadItemsResponse _defaultItem;


    public ItemEditPage()
    {
        InitializeComponent();
        _saveItem = new AppMobileApiMGetDataDownloadItemsResponse();
        _defaultItem = new AppMobileApiMGetDataDownloadItemsResponse();
        DataForm.CommitMode = CommitMode.Manually;
    }

    private async void SaveItemClick(object sender, EventArgs e)
    {
        if (!DataForm.Validate())
            return;
        DataForm.Commit();
        ViewModel.Save();
        _saveItem = (AppMobileApiMGetDataDownloadItemsResponse)DataForm.DataObject;
        _saveItem.Modificado = true;
        if (_saveItem.ItemPk == 0)
        {
            await _repositoryItems.PosMeInsert(_saveItem);
        }
        else
        {
            await _repositoryItems.PosMeUpdate(_saveItem);
        }

        VariablesGlobales.CantidadTransacciones++;
    }


    private void dataForm_ValidateProperty(object sender, DataFormPropertyValidationEventArgs e)
    {
        if (e.PropertyName == "BarCode" && e.NewValue != null)
        {
            e.HasError = true;
            e.ErrorText = "Debe especifcar el código de barra";
        }
    }

    private void DataForm_OnValidateForm(object sender, DataFormValidationEventArgs e)
    {
        _saveItem = (AppMobileApiMGetDataDownloadItemsResponse)e.DataObject;
        if (string.IsNullOrWhiteSpace(_saveItem.ItemNumber))
        {
            e.HasErrors = true;
        }

        if (string.IsNullOrWhiteSpace(_saveItem.BarCode))
        {
            e.HasErrors = true;
        }

        if (string.IsNullOrWhiteSpace(_saveItem.Name))
        {
            e.HasErrors = true;
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
        _saveItem = (AppMobileApiMGetDataDownloadItemsResponse)DataForm.DataObject;
        _saveItem.CantidadFinal = decimal.Add(_saveItem.CantidadEntradas, _saveItem.Quantity) - _saveItem.CantidadSalidas;
        TextCantidadFinal.Text = _saveItem.CantidadFinal.ToString("N");
    }

    protected override async void OnAppearing()
    {
        _saveItem = (AppMobileApiMGetDataDownloadItemsResponse)DataForm.DataObject;
        _defaultItem = await _repositoryItems.PosMeFindByItemNumber(_saveItem.ItemNumber);
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
            TextPrecioPublico.Text = _defaultItem.PrecioPublico.ToString("N");
        }
    }
}