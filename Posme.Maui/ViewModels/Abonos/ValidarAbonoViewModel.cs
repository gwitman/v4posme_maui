using System.Web;
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
        Title = "Aplicar Abono 5/5";
        _parameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
        Item = VariablesGlobales.DtoAplicarAbono;
    }

    public DtoAbono Item { get; private set; }
    private ImageSource _logoSource;

    public ImageSource LogoSource
    {
        get => _logoSource;
        set => SetProperty(ref _logoSource, value);
    }

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