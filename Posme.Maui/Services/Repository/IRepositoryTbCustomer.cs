using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbCustomer : IRepositoryFacade<Api_AppMobileApi_GetDataDownloadCustomerResponse>
{
    Task<Api_AppMobileApi_GetDataDownloadCustomerResponse> PosMeFindCustomer(string customerNumber);

    Task<List<Api_AppMobileApi_GetDataDownloadCustomerResponse>> PosMeFilterBySearch(string search);

    Task<List<Api_AppMobileApi_GetDataDownloadCustomerResponse>> PosMeFilterByInvoice();
}