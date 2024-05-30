using Posme.Maui.Services.Repository;
using System.Runtime.Intrinsics.Arm;

namespace Posme.Maui.Services.Helpers;

public class Helper(IRepositoryTbParameterSystem repositoryParameters)
{
    public async Task<int> GetCounter()
    {
        var find = await repositoryParameters.PosMeFindCounter();
        return Convert.ToInt32(find.Value);
    }

    public async Task PlusCounter()
    {
        var find = await repositoryParameters.PosMeFindCounter();
        var value = Convert.ToInt32(find.Value) + 1;
        find.Value = $"{value}";
        await repositoryParameters.PosMeUpdate(find);
    }

    public async Task<string> ParseUrl(string url)
    {
        var find = await repositoryParameters.PosMeFindAccessPoint();
        var urlTaret = url.Replace(Constantes.UrlBase, find.Value);
        return urlTaret;
    }
}