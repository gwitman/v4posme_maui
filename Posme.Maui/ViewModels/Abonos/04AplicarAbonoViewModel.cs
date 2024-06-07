using System.Diagnostics;
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
    private Api_AppMobileApi_GetDataDownloadDocumentCreditResponse _documentCreditResponse;
    private Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse _documentCreditAmortization;
    private Api_AppMobileApi_GetDataDownloadCustomerResponse _customerResponse;

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

        try
        {
            var codigoAbono = await _helper.GetCodigoAbono();
            _customerResponse = await _repositoryTbCustomer.PosMeFindCustomer(DocumentCreditAmortizationResponse.CustomerNumber!);
            DocumentCreditResponse.BalanceDocument = decimal.Subtract(DocumentCreditResponse.BalanceDocument, Monto);
            VariablesGlobales.DtoAplicarAbono = new ViewTempDtoAbono(
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
            var tmpMonto = Monto;
            var transactionMaster = new TbTransactionMaster
            {
                TransactionId = TypeTransaction.TransactionShare,
                SubAmount = Monto,
                Discount = SaldoInicial,
                Amount = SaldoFinal,
                Comment = Description,
                TransactionNumber = codigoAbono,
                TransactionOn = DateTime.Now,
                EntitySecondaryId = _customerResponse.CustomerNumber,
                EntityId = _customerResponse.EntityId
            };
            _customerResponse.Balance = decimal.Compare(_customerResponse.Balance, Monto) > 0 ? decimal.Subtract(_customerResponse.Balance, Monto) : decimal.Zero;
            var documentCredits = await _repositoryDocumentCreditAmortization.PosMeFilterByCustomerNumber(_customerResponse.CustomerNumber!);
            var tmpListaSave = new List<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse>();
            foreach (var documentCreditAmortization in documentCredits)
            {
                if (decimal.Compare(tmpMonto, decimal.Zero) <= 0)
                {
                    break;
                }

                if (decimal.Compare(documentCreditAmortization.Remaining, tmpMonto) <= 0)
                {
                    tmpMonto = decimal.Subtract(tmpMonto, documentCreditAmortization.Remaining);
                    documentCreditAmortization.Remaining = decimal.Zero;
                }
                else
                {
                    documentCreditAmortization.Remaining = tmpMonto;
                    tmpMonto = decimal.Zero;
                }

                tmpListaSave.Add(documentCreditAmortization);
                transactionMaster.Reference1 = $"{transactionMaster.Reference1},{documentCreditAmortization.DocumentCreditAmortizationId}";
            }

            var taskAmortization = _repositoryDocumentCreditAmortization.PosMeUpdateAll(tmpListaSave);
            var taskDocument = _repositoryDocumentCredit.PosMeUpdate(DocumentCreditResponse);
            var taskCustomer = _repositoryTbCustomer.PosMeUpdate(_customerResponse);
            Task.WaitAll([taskAmortization, taskDocument, taskCustomer]);
            await NavigationService.NavigateToAsync<ValidarAbonoViewModel>(DocumentCreditResponse.DocumentNumber!);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
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

    public Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse DocumentCreditAmortizationResponse
    {
        get => _documentCreditAmortization;
        set => SetProperty(ref _documentCreditAmortization, value);
    }

    public Api_AppMobileApi_GetDataDownloadDocumentCreditResponse DocumentCreditResponse
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