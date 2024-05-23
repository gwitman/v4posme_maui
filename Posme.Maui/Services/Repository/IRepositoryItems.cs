using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryItems : IRepositoryFacade<AppMobileApiMGetDataDownloadItemsResponse>
{
    Task<AppMobileApiMGetDataDownloadItemsResponse?> PosMeFindByBarCode(string barCode);
    Task<AppMobileApiMGetDataDownloadItemsResponse> PosMeFindByItemNumber(string? itemNumber);
    Task<List<AppMobileApiMGetDataDownloadItemsResponse>> PosMeFilterdByItemNumber(string? textSearch);
    Task<List<AppMobileApiMGetDataDownloadItemsResponse>> PosMeFilterdByItemNumberAndBarCodeAndName(string? textSearch);
}