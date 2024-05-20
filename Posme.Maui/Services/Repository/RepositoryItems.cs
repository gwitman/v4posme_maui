using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryItems(DataBase dataBase)
    : RepositoryFacade<AppMobileApiMGetDataDownloadItemsResponse>(dataBase), IRepositoryItems
{
    private readonly DataBase _dataBase = dataBase;

    public async Task<AppMobileApiMGetDataDownloadItemsResponse?> PosMeFindByBarCode(string barCode)
    {
        return await _dataBase.Database.Table<AppMobileApiMGetDataDownloadItemsResponse>()
            .FirstOrDefaultAsync(response => response.BarCode == barCode);
    }

    public async Task<AppMobileApiMGetDataDownloadItemsResponse?> PosMeFindByItemNumber(string itemNumber)
    {
        return await _dataBase.Database.Table<AppMobileApiMGetDataDownloadItemsResponse>()
            .FirstOrDefaultAsync(response => response.ItemNumber == itemNumber);
    }

    public async Task<List<AppMobileApiMGetDataDownloadItemsResponse>> PosMeFilterdByItemNumber(string? textSearch)
    {
        return await _dataBase.Database.Table<AppMobileApiMGetDataDownloadItemsResponse>()
            .Where(response => response.ItemNumber!.Contains(textSearch!))
            .ToListAsync();
    }
}