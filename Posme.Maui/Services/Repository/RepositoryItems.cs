using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryItems(DataBase dataBase)
    : RepositoryFacade<Api_AppMobileApi_GetDataDownloadItemsResponse>(dataBase), IRepositoryItems
{
    private readonly DataBase _dataBase = dataBase;

    public async Task<Api_AppMobileApi_GetDataDownloadItemsResponse?> PosMeFindByBarCode(string barCode)
    {
        return await _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadItemsResponse>()
            .FirstOrDefaultAsync(response => response.BarCode == barCode);
    }

    public async Task<Api_AppMobileApi_GetDataDownloadItemsResponse> PosMeFindByItemNumber(string? itemNumber)
    {
        return await _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadItemsResponse>()
            .FirstOrDefaultAsync(response => response.ItemNumber == itemNumber);
    }

    public async Task<List<Api_AppMobileApi_GetDataDownloadItemsResponse>> PosMeFilterdByItemNumber(string? textSearch)
    {
        return await _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadItemsResponse>()
            .Where(response => response.ItemNumber!.Contains(textSearch!))
            .ToListAsync();
    }

    public async Task<List<Api_AppMobileApi_GetDataDownloadItemsResponse>> PosMeFilterdByItemNumberAndBarCodeAndName(string? textSearch)
    {
        textSearch = textSearch!.ToLower();
        return await _dataBase.Database.Table<Api_AppMobileApi_GetDataDownloadItemsResponse>()
            .Where(response => response.ItemNumber!.ToLower().Contains(textSearch)
                               || response.BarCode.ToLower().Contains(textSearch)
                               || response.Name.ToLower().Contains(textSearch))
            .ToListAsync();
    }
}