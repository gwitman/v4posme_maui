using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbTransactionMaster : IRepositoryFacade<TbTransactionMaster>
{
    Task<List<TbTransactionMaster>> PosMeFilterByCodigoAndNombreClienteFacturas(string filter);
    Task<List<TbTransactionMaster>> PosMeFilterByCodigoAndNombreClienteAbonos(string filter);
    Task<List<TbTransactionMaster>> PosMeFilterFacturas();
    Task<List<TbTransactionMaster>> PosMeFilterAbonos();
    Task<List<TbTransactionMaster>> PosMeFilterAbonosByCustomer(int entityId);
    Task<TbTransactionMaster> PosMeFindByTransactionId(int id);
    Task<TbTransactionMaster> PosMeFindByTransactionNumber(string transactionNumber);
}