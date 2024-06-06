using System.Web;
using CommunityToolkit.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.ViewModels.Abonos;

public class AplicarAbonoViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IRepositoryDocumentCredit _repositoryDocumentCredit;
    private readonly IRepositoryDocumentCreditAmortization _repositoryDocumentCreditAmortization;
    private readonly IRepositoryTbCustomer _repositoryTbCustomer;
    private readonly Helper _helper;
    private AppMobileApiMGetDataDownloadDocumentCreditResponse _documentCreditResponse;
    private AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse _documentCreditAmortization;
    private AppMobileApiMGetDataDownloadCustomerResponse _customerResponse;

    public AplicarAbonoViewModel()
    {
        _documentCreditResponse = new();
        _documentCreditAmortization = new();
        _customerResponse = new();
        _helper = VariablesGlobales.UnityContainer.Resolve<Helper>();
        Title = "Completar Abono 4/5";
        _repositoryDocumentCredit = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();
        _repositoryDocumentCreditAmortization = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCreditAmortization>();
        _repositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        AplicarAbonoCommand = new Command(OnAplicarAbono);
    }

    private async void OnAplicarAbono(object obj)
    {
        if (Validate())
        {
            return;
        }

        var codigoAbono = await _helper.GetCodigoAbono();
        _customerResponse = await _repositoryTbCustomer.PosMeFindCustomer(DocumentCreditAmortizationResponse.CustomerNumber!);
        VariablesGlobales.DtoAplicarAbono = new DtoAbono(
            codigoAbono,
            _customerResponse.CustomerNumber!,
            _customerResponse.FirstName!,
            _customerResponse.LastName!,
            _customerResponse.Identification!,
            DateTime.Now,
            DocumentCreditAmortizationResponse.DocumentNumber!,
            CurrencyName!,
            Monto,
            SaldoInicial,
            SaldoFinal,
            Description!);
        await NavigationService.NavigateToAsync<ValidarAbonoViewModel>("");
    }

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            ShowToast("Especifique una descripción del abono", ToastDuration.Long, 16);
            return true;
        }

        if (decimal.Compare(Monto, decimal.Zero) <= 0)
        {
            ShowToast("Especifique un monto del abono", ToastDuration.Long, 16);
            return true;
        }

        return false;
    }

    public AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse DocumentCreditAmortizationResponse
    {
        get => _documentCreditAmortization;
        set => SetProperty(ref _documentCreditAmortization, value);
    }

    public AppMobileApiMGetDataDownloadDocumentCreditResponse DocumentCreditResponse
    {
        get => _documentCreditResponse;
        set => SetProperty(ref _documentCreditResponse, value);
    }

    public bool MontoError { get; set; }

    public bool DescriptionError { get; set; }

    private string? _currencyName;

    public string? CurrencyName
    {
        get => _currencyName;
        set => SetProperty(ref _currencyName, value);
    }

    private string? _description;

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private decimal _saldoInicial;

    public decimal SaldoInicial
    {
        get => _saldoInicial;
        set => SetProperty(ref _saldoInicial, value);
    }

    private decimal _monto;

    public decimal Monto
    {
        get => _monto;
        set
        {
            SetProperty(ref _monto, value);
            _saldoFinal = decimal.Subtract(SaldoInicial, value);
            OnPropertyChanged(nameof(SaldoFinal));
        }
    }

    private decimal _saldoFinal;

    public decimal SaldoFinal
    {
        get => _saldoFinal;
        set => SetProperty(ref _saldoFinal, value);
    }

    public Command AplicarAbonoCommand { get; }

    public override async Task InitializeAsync(object parameter)
    {
        await LoadInvoices(parameter as string);
    }

    private async Task LoadInvoices(string? parameter)
    {
        if (string.IsNullOrWhiteSpace(parameter))
        {
            return;
        }

        IsBusy = true;
        DocumentCreditResponse = await _repositoryDocumentCredit.PosMeFindDocumentNumber(parameter);
        DocumentCreditAmortizationResponse = await _repositoryDocumentCreditAmortization.PosMeFindByDocumentNumber(parameter);
        CurrencyName = DocumentCreditResponse.CurrencyName!;
        SaldoInicial = DocumentCreditResponse.BalanceDocument;
        IsBusy = false;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
        await LoadInvoices(id);
    }
}