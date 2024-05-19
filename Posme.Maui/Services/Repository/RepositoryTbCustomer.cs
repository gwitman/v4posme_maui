using Posme.Maui.Models;
using SQLite;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbCustomer(DataBase dataBase) : RepositoryFacade<CoreAcountCustomers>(dataBase),IRepositoryTbCustomer
{

    public Task<CoreAcountCustomers> PosMeFindCustomer(string customerNumber)
    {
        return dataBase.Database.Table<CoreAcountCustomers>()
            .Where(customers => customers.CustomerNumber == customerNumber)
            .FirstOrDefaultAsync();
    }

}