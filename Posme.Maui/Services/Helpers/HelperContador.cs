using Posme.Maui.Services.Repository;

namespace Posme.Maui.Services.Helpers;

public class HelperContador(IRepositoryTbParameterSystem repositoryParameters)
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
}