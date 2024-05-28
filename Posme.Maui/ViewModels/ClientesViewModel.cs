using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Maui.Core.Internal;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.ViewModels;

public class ClientesViewModel : BaseViewModel
{
    private IRepositoryTbCustomer _customerRepositoryTbCustomer;
    public ClientesViewModel()
    {
        _customerRepositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        Customers = new DXObservableCollection<AppMobileApiMGetDataDownloadCustomerResponse>();
        SearchCommand = new Command(OnSearchCommand);
        OnBarCode = new Command(OnBarCodeShow);
    }

    private void OnSearchCommand(object obj)
    {
        
    }

    private void OnBarCodeShow(object obj)
    {
        
    }

    private async void LoadsClientes()
    {
        IsBusy = true;
        await Task.Run(async () =>
        {
            Customers.Clear();
            var findAll = await _customerRepositoryTbCustomer.PosMeFindAll();
            foreach (var item in findAll)
            {
                Customers.Add(item);
            }
        });
        IsBusy = false;
    }

    public ICommand OnBarCode { get; }
    public ICommand SearchCommand { get; }
    public DXObservableCollection<AppMobileApiMGetDataDownloadCustomerResponse> Customers { get; }
    public AppMobileApiMGetDataDownloadCustomerResponse SelectedCustomer { get; set; }


    public void OnAppearing(INavigation navigation)
    {
        LoadsClientes();
    }
}