﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Api;

namespace Posme.Maui.ViewModels;

public class PosMeCustomerViewModel : BaseViewModel
{
    private readonly IRepositoryTbCustomer _customerRepositoryTbCustomer;

    public PosMeCustomerViewModel()
    {
        _customerRepositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        Customers = new ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse>();
        SearchCommand = new Command(OnSearchCommand);
        OnBarCode = new Command(OnBarCodeShow);
    }

    public ICommand OnBarCode { get; }
    public ICommand SearchCommand { get; }
    private ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse> _customers;

    public ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse> Customers
    {
        get => _customers;
        set => SetProperty(ref _customers, value);
    }

    private Api_AppMobileApi_GetDataDownloadCustomerResponse? _selectedCustomer;

    public Api_AppMobileApi_GetDataDownloadCustomerResponse? SelectedCustomer
    {
        get => _selectedCustomer;
        set => SetProperty(ref _selectedCustomer, value);
    }

    private async void OnSearchCommand(object obj)
    {
        IsBusy = true;
        await Task.Run(async () =>
        {
            Customers.Clear();
            var finds = await _customerRepositoryTbCustomer.PosMeFilterBySearch(Search);
            Customers = new ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse>(finds);
        });
        IsBusy = false;
    }

    private async void OnBarCodeShow(object obj)
    {
        var barCodePage = new BarCodePage();
        await Navigation!.PushModalAsync(barCodePage);
        if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
        Search = VariablesGlobales.BarCode;
        VariablesGlobales.BarCode = "";
        OnSearchCommand(Search);
    }

    private async void LoadsClientes()
    {
        IsBusy = true;
        await Task.Run(async () =>
        {
            Thread.Sleep(1000);
            var findAll = await _customerRepositoryTbCustomer.PosMeAscTake10();
            Customers = new ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse>(findAll);
        });
        IsBusy = false;
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        LoadsClientes();
    }
}