using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbCustomer : IRepositoryFacade<CoreAcountCustomers>
{
    Task<CoreAcountCustomers> PosMeFindCustomer(string customerNumber);

}