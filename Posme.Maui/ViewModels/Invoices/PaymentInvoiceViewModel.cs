using System.Diagnostics;
using Posme.Maui.Services.SystemNames;

namespace Posme.Maui.ViewModels.Invoices;

public class PaymentInvoiceViewModel : BaseViewModel
{
    public PaymentInvoiceViewModel()
    {
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
        Monto=decimal.Zero;
        Cambio = decimal.Zero;
    }

    private bool OnValidatePago()
    {
        return !Validate();
    }

    private void OnAplicarPagoCommand()
    {
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
    private string _pagarSeleccion;

    public string PagarSeleccion
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