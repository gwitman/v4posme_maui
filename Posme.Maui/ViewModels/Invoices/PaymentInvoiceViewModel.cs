using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Unity;

namespace Posme.Maui.ViewModels.Invoices;

public class PaymentInvoiceViewModel : BaseViewModel
{
    private readonly HelperCore _helper;
    private readonly IRepositoryTbTransactionMasterDetail _repositoryTbTransactionMasterDetail;
    private readonly IRepositoryTbTransactionMaster _repositoryTbTransactionMaster;
    private readonly IRepositoryItems _repositoryItems;

    public PaymentInvoiceViewModel()
    {
        _helper = VariablesGlobales.UnityContainer.Resolve<HelperCore>();
        _repositoryTbTransactionMasterDetail = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMasterDetail>();
        _repositoryTbTransactionMaster = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMaster>();
        _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
        Title = "Pago 5/5";
        SelectionEfectivoCommand = new Command(OnSelectionEfectivoCommand);
        SelectionDebitoCommand = new Command(OnSelectionDebitoCommand);
        SelectionCreditoCommand = new Command(OnSelectionCreditoCommand);
        SelectionMonederoCommand = new Command(OnSelectionMonederoCommand);
        SelectionChequeCommand = new Command(OnSelectionChequeCommand);
        SelectionOtrosCommand = new Command(OnSelectionOtrosCommand);
        AplicarPagoCommand = new Command(OnAplicarPagoCommand, OnValidatePago);
        ClearMontoCommand = new Command(OnClearMontoCommand);
        PagarSeleccion = "Pagar con Selección";
        PropertyChanged += (_, _) => AplicarPagoCommand.ChangeCanExecute();
    }

    private bool Validate()
    {
        return decimal.Compare(Monto, decimal.Zero) <= 0;
    }

    private void OnClearMontoCommand(object obj)
    {
        Monto = decimal.Zero;
        Cambio = decimal.Zero;
    }

    private bool OnValidatePago()
    {
        return !Validate();
    }

    private async void OnAplicarPagoCommand()
    {
        IsBusy = true;
        var dtoInvoice = VariablesGlobales.DtoInvoice;
        var codigo = await _helper.GetCodigoFactura();
        VariablesGlobales.DtoInvoice.Codigo = codigo;
        VariablesGlobales.DtoInvoice.Monto = Monto;
        VariablesGlobales.DtoInvoice.Cambio = Cambio;
        VariablesGlobales.DtoInvoice.TransactionOn = DateTime.Now;
        var transactionMaster = new TbTransactionMaster
        {
            TransactionId = TypeTransaction.TransactionInvoiceBilling,
            Amount = Monto,
            TransactionOn = DateTime.Now,
            TransactionCausalId = 0, //crear enum
            Comment = dtoInvoice.Comentarios,
            Discount = decimal.Zero,
            Taxi1 = decimal.Zero,
            ExchangeRate = decimal.Zero, //definir
            EntityId = dtoInvoice.CustomerResponse!.EntityId,
            EntitySecondaryId = VariablesGlobales.User!.UserId.ToString(),
            TransactionNumber = codigo,
            TipoDocumento = dtoInvoice.TipoDocumento!.Key,
            CurrencyId = dtoInvoice.Currency!.Key
        };
        transactionMaster.SubAmount = dtoInvoice.Balance - transactionMaster.Discount + transactionMaster.Taxi1;

        var listMasterDetail = new List<TbTransactionMasterDetail>();

        foreach (var item in dtoInvoice.Items)
        {
            var findPrecioOriginal = await _repositoryItems.PosMeFindByItemNumber(item.ItemNumber);
            var detail = new TbTransactionMasterDetail
            {
                Quantity = item.Quantity,
                UnitaryCost = findPrecioOriginal.PrecioPublico,
                UnitaryPrice = item.PrecioPublico,
                TransactionMasterId = codigo,
                SubAmount = item.Importe,
                Discount = decimal.Zero,
                Tax1 = decimal.Zero,
                Componentid = 0,
                ComponentItemid = 0
            };
            detail.Amount=detail.SubAmount-detail.Discount+detail.Tax1;
            listMasterDetail.Add(detail);
        }

        await _repositoryTbTransactionMaster.PosMeInsert(transactionMaster);
        await _repositoryTbTransactionMasterDetail.PosMeInsertAll(listMasterDetail);
        await _helper.PlusCounter();
        IsBusy = false;
        await NavigationService.NavigateToAsync<PrinterInvoiceViewModel>();
    }

    private void OnSelectionOtrosCommand()
    {
        ChangedChecked(false, false, false, false, false, true);
        PagarSeleccion = "Pagar con Otros";
    }

    private void OnSelectionChequeCommand()
    {
        ChangedChecked(false, false, false, false, true, false);
        PagarSeleccion = "Pagar con Cheque";
    }

    private void OnSelectionMonederoCommand()
    {
        ChangedChecked(false, false, false, true, false, false);
        PagarSeleccion = "Pagar con Monedero";
    }

    private void OnSelectionCreditoCommand()
    {
        ChangedChecked(false, false, true, false, false, false);
        PagarSeleccion = "Pagar con Credito";
    }

    private void OnSelectionDebitoCommand()
    {
        ChangedChecked(false, true, false, false, false, false);
        PagarSeleccion = "Pagar con Debito";
    }

    private void OnSelectionEfectivoCommand()
    {
        ChangedChecked(true, false, false, false, false, false);
        PagarSeleccion = "Pagar con Efectivo";
    }

    public string Moneda => VariablesGlobales.DtoInvoice.Currency!.Simbolo;
    public decimal Balance => VariablesGlobales.DtoInvoice.Balance;
    private bool _chkEfectivo;

    public bool ChkEfectivo
    {
        get => _chkEfectivo;
        set => SetProperty(ref _chkEfectivo, value);
    }

    private bool _chkCredito;

    public bool ChkCredito
    {
        get => _chkCredito;
        set => SetProperty(ref _chkCredito, value);
    }

    private bool _chkDebito;

    public bool ChkDebito
    {
        get => _chkDebito;
        set => SetProperty(ref _chkDebito, value);
    }

    private bool _chkCheque;

    public bool ChkCheque
    {
        get => _chkCheque;
        set => SetProperty(ref _chkCheque, value);
    }

    private bool _chkMonedero;

    public bool ChkMonedero
    {
        get => _chkMonedero;
        set => SetProperty(ref _chkMonedero, value);
    }

    private bool _chkOtros;

    public bool ChkOtros
    {
        get => _chkOtros;
        set => SetProperty(ref _chkOtros, value);
    }

    private decimal _monto;

    public decimal Monto
    {
        get => _monto;
        set
        {
            SetProperty(ref _monto, value);
            _cambio = decimal.Subtract(value, Balance);
            OnPropertyChanged(nameof(Cambio));
        }
    }

    private decimal _cambio;

    public decimal Cambio
    {
        get => _cambio;
        set => SetProperty(ref _cambio, value);
    }

    public Command SelectionEfectivoCommand { get; }
    public Command SelectionDebitoCommand { get; }
    public Command SelectionCreditoCommand { get; }
    public Command SelectionMonederoCommand { get; }
    public Command SelectionChequeCommand { get; }
    public Command SelectionOtrosCommand { get; }
    private string? _pagarSeleccion;

    public string? PagarSeleccion
    {
        get => _pagarSeleccion;
        set => SetProperty(ref _pagarSeleccion, value);
    }

    public Command AplicarPagoCommand { get; }
    public Command ClearMontoCommand { get; }

    private void ChangedChecked(bool efectivo, bool debito, bool credito, bool monedero, bool cheque, bool otros)
    {
        ChkEfectivo = efectivo;
        ChkCredito = credito;
        ChkDebito = debito;
        ChkCheque = cheque;
        ChkMonedero = monedero;
        ChkOtros = otros;
    }
}