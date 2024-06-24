using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbTransactionMasterDetail : IRepositoryFacade<TbTransactionMasterDetail>
{
    Task<List<TbTransactionMasterDetail>> PosMeItemByTransactionId(int transactionId);
}