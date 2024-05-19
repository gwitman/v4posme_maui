using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbCustomer : IRepositoryFacade<AppMobileApiMGetDataDownloadCustomerResponse>
{
    Task<AppMobileApiMGetDataDownloadCustomerResponse> PosMeFindCustomer(string customerNumber);

}