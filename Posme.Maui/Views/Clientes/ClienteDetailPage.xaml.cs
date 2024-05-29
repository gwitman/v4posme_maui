using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Views.Clientes;

public partial class ClienteDetailPage : ContentPage
{
    private readonly IRepositoryTbCustomer _repositoryTbCustomer;
    private DetailFormViewModel ViewModel => (DetailFormViewModel)BindingContext;
    private AppMobileApiMGetDataDownloadCustomerResponse SelectedItem => (AppMobileApiMGetDataDownloadCustomerResponse)ViewModel.Item;
    private bool _isDeleting;

    public ClienteDetailPage()
    {
        InitializeComponent();
        _repositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
    }

    private void DeleteItemClick(object? sender, EventArgs e)
    {
        Popup.IsOpen = true;
    }

    private async void DeleteConfirmedClick(object? sender, EventArgs e)
    {
        if (_isDeleting)
            return;
        _isDeleting = true;

        try
        {
            _isDeleting = await _repositoryTbCustomer.PosMeDelete(SelectedItem);
            ViewModel.Close();
        }
        catch (Exception ex)
        {
            _isDeleting = false;
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void CancelDeleteClick(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }
}