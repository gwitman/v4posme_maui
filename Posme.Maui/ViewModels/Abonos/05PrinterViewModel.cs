using System.Web;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using SkiaSharp;
using Unity;
using Posme.Maui.Services.Api;
using Posme.Maui.Services.SystemNames;
using Printer = Posme.Maui.Services.HelpersPrinters.Printer;

namespace Posme.Maui.ViewModels.Abonos;

public class ValidarAbonoViewModel : BaseViewModel
{
    private readonly IRepositoryTbParameterSystem _parameterSystem;
    private readonly IRepositoryTbTransactionMaster _repositoryTbTransactionMaster;

    public ValidarAbonoViewModel()
    {
        Title = "Comprobanto de Abono 5/5";
        _parameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        _repositoryTbTransactionMaster = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMaster>();
        Item = VariablesGlobales.DtoAplicarAbono!;
        AplicarOtroCommand = new Command(OnAplicarOtroCommand);
        PrintCommand = new Command(OnPrintCommand);
        AnularCommand = new Command(OnAnularCommand);
    }

    private async void OnAnularCommand()
    {
        IsBusy = true;
        var findAbonos = await _repositoryTbTransactionMaster.PosMeFilterAbonosByCustomer(Item.EntityId);
        if (findAbonos.First().TransactionNumber != Item.CodigoAbono)
        {
            ShowToast(Mensajes.AnularAbonoValidacion, ToastDuration.Long, 18);
            IsBusy = false;
            return;
        }

        await HelperCustomerCreditDocumentAmortization.AnularAbono(Item.CodigoAbono);
        IsBusy = false;
        OnAplicarOtroCommand();
    }

    private async void OnPrintCommand(object obj)
    {
        var parametroPrinter = await _parameterSystem.PosMeFindPrinter();
        var logo = await _parameterSystem.PosMeFindLogo();
        if (string.IsNullOrWhiteSpace(parametroPrinter.Value))
        {
            return;
        }

        var printer = new Printer(parametroPrinter.Value);
        var readImage = Convert.FromBase64String(logo.Value!);
        printer.AlignRight();
        printer.Image(SKBitmap.Decode(readImage));
        printer.AlignCenter();
        printer.BoldMode("FERRETERIA NARVAEZ");
        printer.BoldMode($"ABONO: {VariablesGlobales.DtoAplicarAbono!.CodigoAbono}");
        printer.NewLine();
        printer.AlignLeft();
        printer.Append($"Le informamos: \n{VariablesGlobales.DtoAplicarAbono.FirstName} {VariablesGlobales.DtoAplicarAbono.LastName} " +
                       $"con número de cedula {VariablesGlobales.DtoAplicarAbono.Identification} ha realizado un abono a su cuenta.");
        printer.NewLine();
        printer.Append($"Fecha            : {VariablesGlobales.DtoAplicarAbono.Fecha:yyyy-MM-dd}");
        printer.Append($"Saldo inicial    : {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoInicial:N2}");
        printer.Append($"Monto de abono   : {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.MontoAplicar:N2}");
        printer.Append($"Saldo final      : {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoFinal:N2}");
        printer.NewLine();
        printer.Append($"Comentarios: {VariablesGlobales.DtoAplicarAbono.Description}");
        printer.NewLine();
        printer.AlignCenter();
        printer.Append("Información, datos de muestra");
        printer.Append("Dirección: dirección de muestra para información de abono");
        printer.FullPaperCut();
        printer.Print();
    }

    private void OnAplicarOtroCommand()
    {
        var stack = Shell.Current.Navigation.NavigationStack.ToArray();
        for (var i = stack.Length - 1; i > 0; i--)
        {
            Shell.Current.Navigation.RemovePage(stack[i]);
        }
    }

    public ViewTempDtoAbono Item { get; private set; }

    private ImageSource? _logoSource;

    public ImageSource? LogoSource
    {
        get => _logoSource;
        set => SetProperty(ref _logoSource, value);
    }

    public ICommand AplicarOtroCommand { get; }

    public ICommand PrintCommand { get; }
    public Command AnularCommand { get; }

    public override async Task InitializeAsync(object parameter)
    {
        await OnAppearing(Navigation!);
    }


    public async Task OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        await Task.Run(async () =>
        {
            var paramter = await _parameterSystem.PosMeFindLogo();
            var imageBytes = Convert.FromBase64String(paramter.Value!);
            LogoSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            Item = VariablesGlobales.DtoAplicarAbono!;
        });
    }
}