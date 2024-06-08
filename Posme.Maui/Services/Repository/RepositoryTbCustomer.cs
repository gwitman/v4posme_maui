using Posme.Maui.Models;
using SQLite;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbCustomer(DataBase dataBase) : RepositoryFacade<Api_AppMobileApi_GetDataDownloadCustomerResponse>(dataBase), IRepositoryTbCustomer
{
    private readonly DataBase _dataBase = dataBase;

    public Task<Api_AppMobileApi_GetDataDownloadCustomerResponse> PosMeFindCustomer(string customerNumber)
    {
        return _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadCustomerResponse>()
            .Where(customers => customers.CustomerNumber == customerNumber)
            .FirstOrDefaultAsync();
    }

    public Task<Api_AppMobileApi_GetDataDownloadCustomerResponse> PosMeFindEntityID(int entityID)
    {
        return _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadCustomerResponse>()
            .Where(customers => customers.EntityId == entityID)
            .FirstOrDefaultAsync();
    }

    public Task<List<Api_AppMobileApi_GetDataDownloadCustomerResponse>> PosMeFilterBySearch(string search)
    {
        search = search.ToLower();
        return _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadCustomerResponse>()
            .Where(response => response.CustomerNumber!.ToLower().Contains(search)
                               || response.Identification!.ToLower().Contains(search)
                               || response.FirstName!.ToLower().Contains(search)
                               || response.LastName!.ToLower().Contains(search))
            .ToListAsync();
    }

    public async Task<List<Api_AppMobileApi_GetDataDownloadCustomerResponse>> PosMeFilterByInvoice()
    {
        var query = """
                    select distinct tbc.CustomerId, ComapnyId, BranchId, tbc.EntityId, CustomerNumber, Identification, 
                                    FirstName, LastName, tbc.Balance, Modificado 
                    from tb_customers tbc join tb_document_credit tdc on tbc.EntityId = tdc.EntityId
                    """;
        return await _dataBase.Database.QueryAsync<Api_AppMobileApi_GetDataDownloadCustomerResponse>(query);
    }
}