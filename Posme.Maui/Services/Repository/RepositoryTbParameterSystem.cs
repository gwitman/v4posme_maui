using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbParameterSystem(DataBase dataBase) : RepositoryFacade<TbParameterSystem>(dataBase), IRepositoryTbParameterSystem
{
    public Task<TbParameterSystem> PosMeFindLogo()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .Where(system => system.Name == Constantes.ParametroLogo)
            .FirstOrDefaultAsync();
    }

    public Task<TbParameterSystem> PosMeFindCounter()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .Where(system => system.Name == Constantes.ParametroCounter)
            .FirstOrDefaultAsync();
    }

    public Task<TbParameterSystem> PosMeFindAccessPoint()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .Where(system => system.Name == Constantes.ParametroAccesPoint)
            .FirstOrDefaultAsync();
    }

    public Task<TbParameterSystem> PosMeFindPrinter()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .Where(system => system.Name == Constantes.ParametroPrinter)
            .FirstOrDefaultAsync();
    }

    public Task<TbParameterSystem> PosMeFindCodigoAbono()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .Where(system => system.Name == Constantes.ParametroCodigoAbono)
            .FirstOrDefaultAsync();
    }
}