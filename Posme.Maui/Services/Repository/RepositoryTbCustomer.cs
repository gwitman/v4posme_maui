using Posme.Maui.Models;
using SQLite;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbCustomer(DataBase dataBase) : RepositoryFacade<AppMobileApiMGetDataDownloadCustomerResponse>(dataBase),IRepositoryTbCustomer
{

    public Task<AppMobileApiMGetDataDownloadCustomerResponse> PosMeFindCustomer(string customerNumber)
    {
        return dataBase.Database.Table<AppMobileApiMGetDataDownloadCustomerResponse>()
            .Where(customers => customers.CustomerNumber == customerNumber)
            .FirstOrDefaultAsync();
    }

    public Task<List<AppMobileApiMGetDataDownloadCustomerResponse>> PosMeFilterBySearch(string search)
    {
        search = search.ToLower();
        return dataBase.Database.Table<AppMobileApiMGetDataDownloadCustomerResponse>()
            .Where(response => response.CustomerNumber!.ToLower().Contains(search)
                               || response.Identification!.ToLower().Contains(search)
                               || response.FirstName!.ToLower().Contains(search)
                               || response.LastName!.ToLower().Contains(search))
            .ToListAsync();
    }
}