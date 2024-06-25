using Posme.Maui.Models;
using Posme.Maui.Services.SystemNames;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbTransactionMaster(DataBase dataBase) : RepositoryFacade<TbTransactionMaster>(dataBase), IRepositoryTbTransactionMaster
{
    private readonly DataBase _dataBase = dataBase;

    public Task<List<TbTransactionMaster>> PosMeFilterByCodigoAndNombreClienteFacturas(string filter)
    {
        var param = (int)TypeTransaction.TransactionInvoiceBilling;
        var query = $"""
                     select tm.TransactionId,
                             tm.TransactionMasterId,
                             tm.TransactionNumber,
                             tm.EntityId,
                             tm.TransactionOn,
                             tm.EntitySecondaryId,
                             tm.SubAmount,
                             tm.Discount,
                             tm.Taxi1,
                             tm.Amount,
                             tm.TransactionCausalId,
                             tm.ExchangeRate,
                             tm.CurrencyId,
                             tm.Comment,
                             tm.Reference1,
                             tm.Reference2,
                             tm.Reference3
                     from tb_transaction_master tm
                              join tb_customers c on tm.EntityId = c.EntityId
                     where tm.TransactionNumber like '%{filter}%' or c.FirstName like '%{filter}%'
                            and tm.TransactionId =?
                     order by tm.TransactionOn DESC 
                     """;
        return _dataBase.Database.QueryAsync<TbTransactionMaster>(query,param);
    }

    public Task<List<TbTransactionMaster>> PosMeFilterByCodigoAndNombreClienteAbonos(string filter)
    {
        var param = (int)TypeTransaction.TransactionShare;
        var query = $"""
                     select tm.TransactionId,
                             tm.TransactionMasterId,
                             tm.TransactionNumber,
                             tm.EntityId,
                             tm.TransactionOn,
                             tm.EntitySecondaryId,
                             tm.SubAmount,
                             tm.Discount,
                             tm.Taxi1,
                             tm.Amount,
                             tm.TransactionCausalId,
                             tm.ExchangeRate,
                             tm.CurrencyId,
                             tm.Comment,
                             tm.Reference1,
                             tm.Reference2,
                             tm.Reference3
                     from tb_transaction_master tm
                              join tb_customers c on tm.EntityId = c.EntityId
                     where tm.TransactionNumber like '%{filter}%' or c.FirstName like '%{filter}%'
                            and tm.TransactionId =?
                     order by tm.TransactionOn DESC 
                     """;
        return _dataBase.Database.QueryAsync<TbTransactionMaster>(query, param);
    }

    public Task<List<TbTransactionMaster>> PosMeFilterFacturas()
    {
        return _dataBase.Database.Table<TbTransactionMaster>()
            .Where(master => master.TransactionId == TypeTransaction.TransactionInvoiceBilling)
            .OrderByDescending(master=>master.TransactionOn)
            .Take(10)
            .ToListAsync();
    }

    public Task<List<TbTransactionMaster>> PosMeFilterAbonos()
    {
        return _dataBase.Database.Table<TbTransactionMaster>()
            .Where(master => master.TransactionId == TypeTransaction.TransactionShare)
            .OrderByDescending(master=>master.TransactionOn)
            .Take(10)
            .ToListAsync();
    }

    public Task<TbTransactionMaster> PosMeFindByTransactionId(int id)
    {
        return _dataBase.Database.Table<TbTransactionMaster>()
            .FirstOrDefaultAsync(master => master.TransactionMasterId == id);
    }
}