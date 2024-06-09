using System.Web;
using System.Windows.Input;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
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

    public override async Task InitializeAsync(object parameter)
    {
        await OnAppearing();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = HttpUtility.UrlDecode(query["id"] as string);
        await OnAppearing();
    }

    public async Task OnAppearing()
    {
        await Task.Run(async () =>
        {
            var paramter = await _parameterSystem.PosMeFindLogo();
            var imageBytes = Convert.FromBase64String(paramter.Value!);
            LogoSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            Item = VariablesGlobales.DtoAplicarAbono;
        });
    }
}