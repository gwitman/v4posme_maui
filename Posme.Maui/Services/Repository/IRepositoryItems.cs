using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryItems : IRepositoryFacade<Api_AppMobileApi_GetDataDownloadItemsResponse>
{
    Task<Api_AppMobileApi_GetDataDownloadItemsResponse?> PosMeFindByBarCode(string barCode);
    Task<Api_AppMobileApi_GetDataDownloadItemsResponse> PosMeFindByItemNumber(string? itemNumber);
    Task<List<Api_AppMobileApi_GetDataDownloadItemsResponse>> PosMeFilterdByItemNumber(string? textSearch);
    Task<List<Api_AppMobileApi_GetDataDownloadItemsResponse>> PosMeFilterdByItemNumberAndBarCodeAndName(string? textSearch);
}