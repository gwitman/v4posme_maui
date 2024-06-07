using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels.Abonos;

public class AbonosViewModel : BaseViewModel
{
    private readonly IRepositoryTbCustomer _customerRepositoryTbCustomer;
    private readonly IRepositoryDocumentCredit _repositoryDocumentCredit;

    public AbonosViewModel()
    {
        Title = "Selección de cliente 1/5";
        _customerRepositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        _repositoryDocumentCredit = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();
        Customers = new();
        SearchCommand = new Command(OnSearchCommand);
        OnBarCode = new Command(OnBarCodeShow);
        ItemTapped = new Command<Api_AppMobileApi_GetDataDownloadCustomerResponse>(OnItemSelected);
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

    private async void OnItemSelected(Api_AppMobileApi_GetDataDownloadCustomerResponse? item)
    {
        if (item is null)
        {
            return;
        }

        IsBusy = true;
        var invoices = await _repositoryDocumentCredit.PosMeFindByEntityId(item.EntityId);
        if (invoices.Count == 0)
        {
            ShowToast(Mensajes.MensajeDocumentCreditCustomerVacio, ToastDuration.Short, 14);
            return;
        }

        await NavigationService.NavigateToAsync<CustomerDetailInvoiceViewModel>(item.EntityId);
        IsBusy = false;
    }

    private async void OnSearchCommand(object obj)
    {
        IsBusy = true;
        await Task.Run(async () =>
        {
            Customers.Clear();
            var finds = await _customerRepositoryTbCustomer.PosMeFilterBySearch(Search);
            foreach (var customer in finds)
            {
                Customers.Add(customer);
            }
        });
        IsBusy = false;
    }

    public ICommand SearchCommand { get; }
    public ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse> Customers { get; }

    private Api_AppMobileApi_GetDataDownloadCustomerResponse? _selectedCustomer;

    public Api_AppMobileApi_GetDataDownloadCustomerResponse? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            SetProperty(ref _selectedCustomer, value);
            OnItemSelected(value!);
            OnPropertyChanged();
        }
    }

    public Command<Api_AppMobileApi_GetDataDownloadCustomerResponse> ItemTapped { get; }
    public ICommand OnBarCode { get; }

    private async void LoadsClientes()
    {
        IsBusy = true;
        await Task.Run(async () =>
        {
            Customers.Clear();
            var findAll = await _customerRepositoryTbCustomer.PosMeFilterByInvoice();
            foreach (var response in findAll)
            {
                Customers.Add(response);
            }
        });
        IsBusy = false;
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        LoadsClientes();
    }
}