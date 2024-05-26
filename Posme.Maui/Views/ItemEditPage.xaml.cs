using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataForm;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Views;

public partial class ItemEditPage : ContentPage
{
    DetailEditFormViewModel ViewModel => (DetailEditFormViewModel)BindingContext;
    private readonly IRepositoryItems _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
    private AppMobileApiMGetDataDownloadItemsResponse _saveItem;
    
    public ItemEditPage()
    {
        InitializeComponent();
        _saveItem = new AppMobileApiMGetDataDownloadItemsResponse();
    }

    private async void SaveItemClick(object sender, EventArgs e)
    {
        if (!DataForm.Validate())
            return;
        DataForm.Commit();
        ViewModel.Save();
        _saveItem = (AppMobileApiMGetDataDownloadItemsResponse)DataForm.DataObject;
        if (_saveItem.ItemPk==0)
        {
            await _repositoryItems.PosMeInsert(_saveItem);
        }
        else
        {
            await _repositoryItems.PosMeUpdate(_saveItem);    
        }
    }


    private void dataForm_ValidateProperty(object sender, DataFormPropertyValidationEventArgs e)
    {
        if (e.PropertyName=="BarCode" && e.NewValue != null)
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
}