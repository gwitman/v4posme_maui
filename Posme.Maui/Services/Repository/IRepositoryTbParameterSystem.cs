using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbParameterSystem : IRepositoryFacade<TbParameterSystem>
{
    Task<TbParameterSystem> PosMeFindLogo();
    Task<TbParameterSystem> PosMeFindCounter();
    Task<TbParameterSystem> PosMeFindAccessPoint();
    Task<TbParameterSystem> PosMeFindPrinter();
    Task<TbParameterSystem> PosMeFindCodigoAbono();
    Task<TbParameterSystem> PosMeFindCodigoFactura();
}