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

public partial class CustomerEditPage : ContentPage
{
    private DetailEditFormViewModel ViewModel => (DetailEditFormViewModel)BindingContext;
    private static IRepositoryTbCustomer RepositoryTbCustomer => VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
    private readonly HelperContador _helperContador;
    public CustomerEditPage()
    {
        InitializeComponent();
        _helperContador = VariablesGlobales.UnityContainer.Resolve<HelperContador>();
    }

    private async void BarCodeOnClicked(object? sender, EventArgs e)
    {
        var barCodePage = new BarCodePage();
        await Navigation.PushModalAsync(barCodePage);
        if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
        TxtBarCode.Text = VariablesGlobales.BarCode;
        VariablesGlobales.BarCode = "";
    }

    private async void SaveItemClick(object? sender, EventArgs e)
    {
        if (!DataForm.Validate())
            return;
        
        var saveCustomer = (AppMobileApiMGetDataDownloadCustomerResponse)DataForm.DataObject;
        saveCustomer.Modificado = true;
        if (ViewModel.IsNew)
        {
            await RepositoryTbCustomer.PosMeInsert(saveCustomer);
        }
        else
        {
            await RepositoryTbCustomer.PosMeUpdate(saveCustomer);
        }

        await _helperContador.PlusCounter();
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