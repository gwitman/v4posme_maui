using System.Collections.ObjectModel;
using System.Windows.Input;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels.Invoices;

public class InvoicesViewModel : BaseViewModel
{
    private readonly IRepositoryTbCustomer _customerRepositoryTbCustomer;
    public ICommand ItemTapped { get; }
    public ObservableCollection<Api_AppMobileApi_GetDataDownloadCustomerResponse> Customers { get; }
    public ICommand SearchCommand { get; }
    public ICommand OnBarCode { get; }
    public Api_AppMobileApi_GetDataDownloadCustomerResponse? SelectedCustomer { get; set; }

    public InvoicesViewModel()
    {
        Title = "Selección de cliente 1/5";
        _customerRepositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        ItemTapped = new Command<Api_AppMobileApi_GetDataDownloadCustomerResponse>(OnItemTapped);
        SearchCommand = new Command(OnSearchCommand);
        OnBarCode = new Command(OnBarCodeShow);
        Customers = new();
    }

    private async void OnBarCodeShow()
    {
        var barCodePage = new BarCodePage();
        await Navigation!.PushModalAsync(barCodePage);
        var bar = await barCodePage.WaitForResultAsync();
        Search = bar!;
        if (string.IsNullOrWhiteSpace(Search)) return;
        OnSearchCommand(Search);
    }

    private async void OnItemTapped(Api_AppMobileApi_GetDataDownloadCustomerResponse? item)
    {
        if (item is null)
        {
            return;
        }

        IsBusy = true;
        VariablesGlobales.DtoInvoice = new ViewTempDtoInvoice
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            Balance = item.Balance
        };
        await NavigationService.NavigateToAsync<DataInvoicesViewModel>(item.CustomerNumber!);
        IsBusy = false;
    }

    private async void OnSearchCommand(object obj)
    {
        IsBusy = true;
        Customers.Clear();
        List<Api_AppMobileApi_GetDataDownloadCustomerResponse> finds;
        if (string.IsNullOrWhiteSpace(Search))
        {
            finds = await _customerRepositoryTbCustomer.PosMeAscTake10();
        }
        else
        {
            finds = await _customerRepositoryTbCustomer.PosMeFilterByCustomerInvoice(Search);
        }

        foreach (var customer in finds)
        {
            Customers.Add(customer);
        }

        IsBusy = false;
    }

    public async void LoadsClientes()
    {
        Customers.Clear();
        var findAll = await _customerRepositoryTbCustomer.PosMeAscTake10();
        foreach (var response in findAll)
        {
            Customers.Add(response);
        }

        IsBusy = false;
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
    }
}