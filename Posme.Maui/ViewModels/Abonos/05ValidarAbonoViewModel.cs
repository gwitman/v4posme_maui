using System.Diagnostics;
using System.Text;
using System.Web;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using ESC_POS_USB_NET.Printer;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Posme.Maui.Models;
using Posme.Maui.Services;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views.Abonos;
using SkiaSharp;
using Unity;

namespace Posme.Maui.ViewModels.Abonos;

public class ValidarAbonoViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IRepositoryTbParameterSystem _parameterSystem;

    public ValidarAbonoViewModel()
    {
        Title = "Comprobanto de Abono 5/5";
        _parameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        Item = VariablesGlobales.DtoAplicarAbono;
        AplicarOtroCommand = new Command(OnAplicarOtroCommand);
        PrintCommand = new Command(OnPrintCommand);
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
        printer.AlignCenter();
        printer.Image(SKBitmap.Decode(readImage));
        printer.AlignCenter();
        printer.BoldMode("FERRETERIA NARVAEZ");
        printer.NewLine();
        printer.AlignLeft();
        printer.Append($"Le informamos que: \n{VariablesGlobales.DtoAplicarAbono!.FirstName} {VariablesGlobales.DtoAplicarAbono.LastName} creó un código para abono de factura con los siguientes datos");
        printer.NewLine();
        printer.Append($"Código de abono: {VariablesGlobales.DtoAplicarAbono.CodigoAbono}");
        printer.Append($"     N°. Cedula: {VariablesGlobales.DtoAplicarAbono.Identification}");
        printer.Append($"          Fecha: {VariablesGlobales.DtoAplicarAbono.Fecha:yyyy-M-d}");
        printer.Append($"  Saldo inicial: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoInicial:N2}");
        printer.Append($" Monto de abono: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.MontoAplicar:N2}");
        printer.Append($"    Saldo Final: {VariablesGlobales.DtoAplicarAbono.CurrencyName} {VariablesGlobales.DtoAplicarAbono.SaldoFinal:N2}");
        printer.NewLine();
        printer.Append($"Comentarios: {VariablesGlobales.DtoAplicarAbono.Description}");
        printer.NewLine();
        printer.FullPaperCut();
        printer.Print();
    }

    private async void OnAplicarOtroCommand()
    {
        var stack = Shell.Current.Navigation.NavigationStack.ToArray();
        for (var i = stack.Length - 1; i > 0; i--)
        {
            Shell.Current.Navigation.RemovePage(stack[i]);
        }
    }

    public ViewTempDtoAbono Item { get; private set; }
    private ImageSource _logoSource;

    public ImageSource LogoSource
    {
        get => _logoSource;
        set => SetProperty(ref _logoSource, value);
    }

    public ICommand AplicarOtroCommand { get; }
    public ICommand PrintCommand { get; }

    public override async Task InitializeAsync(object parameter)
    {
        await OnAppearing(Navigation);
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
        await OnAppearing(Navigation);
    }

    public async Task OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        await Task.Run(async () =>
        {
            var paramter = await _parameterSystem.PosMeFindLogo();
            var imageBytes = Convert.FromBase64String(paramter.Value!);
            LogoSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            Item = VariablesGlobales.DtoAplicarAbono;
        });
    }


    private string GetFilePath(string filename)
    {
        var folderPath = Environment.GetFolderPath(DeviceInfo.Platform == DevicePlatform.Android
            ? Environment.SpecialFolder.LocalApplicationData
            : Environment.SpecialFolder.MyDocuments);

        return Path.Combine(folderPath, filename);
    }
}