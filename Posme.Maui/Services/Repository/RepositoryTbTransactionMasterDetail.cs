using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbTransactionMasterDetail(DataBase dataBase) : RepositoryFacade<TbTransactionMasterDetail>(dataBase), IRepositoryTbTransactionMasterDetail
{
    private readonly DataBase _dataBase = dataBase;

    public Task<List<TbTransactionMasterDetail>> PosMeItemByTransactionId(int transactionId)
    {
        return _dataBase.Database.Table<TbTransactionMasterDetail>()
            .Where(
                detail => detail.TransactionMasterId == transactionId)
            .ToListAsync();
    }
}