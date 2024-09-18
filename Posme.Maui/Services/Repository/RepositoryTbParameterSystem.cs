using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Api;
namespace Posme.Maui.Services.Repository;

public class RepositoryTbParameterSystem(DataBase dataBase) : RepositoryFacade<TbParameterSystem>(dataBase), IRepositoryTbParameterSystem
{
    public Task<TbParameterSystem> PosMeFindLogo()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroLogo);
    }

    public Task<TbParameterSystem> PosMeFindCounter()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroCounter);
    }
   
    public Task<TbParameterSystem> PosMeFindPrinter()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroPrinter);
    }

    public Task<TbParameterSystem> PosMeFindCodigoAbono()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroCodigoAbono);
    }

    public Task<TbParameterSystem> PosMeFindCodigoFactura()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParameterCodigoFactura);
    }
}