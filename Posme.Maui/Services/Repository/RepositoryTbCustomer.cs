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

}