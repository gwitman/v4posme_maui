using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataForm;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Views.Clientes;

public partial class ClienteEditPage : ContentPage
{
    private DetailEditFormViewModel ViewModel => (DetailEditFormViewModel)BindingContext;
    private static IRepositoryTbCustomer RepositoryTbCustomer => VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
    
    public ClienteEditPage()
    {
        InitializeComponent();
    }

    private async void BarCodeOnClicked(object? sender, EventArgs e)
    {
        var barCodePage = new BarCodePage();
        await Navigation.PushModalAsync(barCodePage);
        if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
        TxtBarCode.Text = VariablesGlobales.BarCode;
        VariablesGlobales.BarCode = "";
    }

    private void SaveItemClick(object? sender, EventArgs e)
    {
        if (!DataForm.Validate())
            return;
        
        var saveCustomer = (AppMobileApiMGetDataDownloadCustomerResponse)DataForm.DataObject;
        saveCustomer.Modificado = true;
        if (ViewModel.IsNew)
        {
            RepositoryTbCustomer.PosMeInsert(saveCustomer);
        }
        else
        {
            RepositoryTbCustomer.PosMeUpdate(saveCustomer);
        }

        VariablesGlobales.CantidadTransacciones++;
        DataForm.Commit();
        ViewModel.Save();
    }

    private void DataForm_OnValidateForm(object sender, DataFormValidationEventArgs e)
    {
        var saveCustomer = (AppMobileApiMGetDataDownloadCustomerResponse)e.DataObject;
        if (string.IsNullOrWhiteSpace(saveCustomer.Identification)
            || string.IsNullOrWhiteSpace(saveCustomer.FirstName)
            || string.IsNullOrWhiteSpace(saveCustomer.LastName))
        {
            e.HasErrors = true;
        }
    }
}