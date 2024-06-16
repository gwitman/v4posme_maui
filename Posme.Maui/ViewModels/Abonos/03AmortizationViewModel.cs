﻿using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Api;
namespace Posme.Maui.ViewModels.Abonos;

public class CreditDetailInvoiceViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IRepositoryDocumentCredit _repositoryDocumentCredit;
    private readonly IRepositoryDocumentCreditAmortization _repositoryDocumentCreditAmortization;
    public ObservableCollection<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse> Items { get; }
    public Command<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse> SwipeCommand { get; }

    public CreditDetailInvoiceViewModel()
    {
        Title = "Selección de cuota 3/5";
        _repositoryDocumentCredit = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();
        _repositoryDocumentCreditAmortization = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCreditAmortization>();
        Items = new();
        SwipeCommand = new Command<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse>(OnSwipeCommand);
    }

    private async void OnSwipeCommand(Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse? item)
    {
        if (item is null)
        {
            return;
        }
        await NavigationService.NavigateToAsync<AplicarAbonoViewModel>(item.DocumentNumber!);
    }

    public override async Task InitializeAsync(object parameter)
    {
        await LoadInvoices(parameter as string);
    }

    private async Task LoadInvoices(string? parameter)
    {
        IsBusy = true;
        var find = await _repositoryDocumentCreditAmortization.PosMeFilterByDocumentNumber(parameter!);
        Items.Clear();
        foreach (var response in find)
        {
            Items.Add(response);
        }

        IsBusy = false;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
        await LoadInvoices(id);
    }
}