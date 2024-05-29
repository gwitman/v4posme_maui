using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Maui.Core.Internal;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels;

public class ClientesViewModel : BaseViewModel
{
    private readonly IRepositoryTbCustomer _customerRepositoryTbCustomer;

    public ClientesViewModel()
    {
        _customerRepositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        Customers = new ObservableCollection<AppMobileApiMGetDataDownloadCustomerResponse>();
        SearchCommand = new Command(OnSearchCommand);
        OnBarCode = new Command(OnBarCodeShow);
    }

    public ICommand OnBarCode { get; }
    public ICommand SearchCommand { get; }
    public ObservableCollection<AppMobileApiMGetDataDownloadCustomerResponse> Customers { get; set; }
    public AppMobileApiMGetDataDownloadCustomerResponse SelectedCustomer { get; set; }

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
            Customers.Clear();
            var findAll = await _customerRepositoryTbCustomer.PosMeFindAll();
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