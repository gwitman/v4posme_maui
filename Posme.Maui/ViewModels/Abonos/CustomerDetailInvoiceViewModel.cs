using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.ViewModels.Abonos;

public class CustomerDetailInvoiceViewModel : BaseViewModel, IQueryAttributable
{
    private IRepositoryDocumentCredit _repositoryDocumentCredit;

    public CustomerDetailInvoiceViewModel()
    {
        _repositoryDocumentCredit = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();
        Invoices = new();
        SearchCommand = new Command(OnSearchCommand);
    }

    private void OnSearchCommand(object obj)
    {
    }

    public ObservableCollection<AppMobileApiMGetDataDownloadDocumentCreditResponse> Invoices { get; }
    public ICommand SearchCommand { get; }

    private AppMobileApiMGetDataDownloadDocumentCreditResponse? _selectedInvoice;

    public AppMobileApiMGetDataDownloadDocumentCreditResponse? SelectedInvoice
    {
        get => _selectedInvoice;
        set
        {
            SetProperty(ref _selectedInvoice, value);
            OnPropertyChanged();
        }
    }

    private async Task LoadInvoices(string? param)
    {
        IsBusy = true;
        Invoices.Clear();
        var invoicesEntityId = await _repositoryDocumentCredit.PosMeFindByEntityId(Convert.ToInt32(param));
        if (invoicesEntityId.Count == 0)
        {
            return;
        }

        foreach (var item in invoicesEntityId)
        {
            Invoices.Add(item);
        }

        IsBusy = false;
    }

    public override async Task InitializeAsync(object parameter)
    {
        await LoadInvoices(parameter as string);
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
        await LoadInvoices(id);
    }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
    }
}