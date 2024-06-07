using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryDocumentCredit(DataBase dataBase) : RepositoryFacade<AppMobileApiMGetDataDownloadDocumentCreditResponse>(dataBase), IRepositoryDocumentCredit
{
    public async Task<List<AppMobileApiMGetDataDownloadDocumentCreditResponse>> PosMeFindByEntityId(int entityId)
    {
        return await dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditResponse>()
            .Where(response => response.EntityId == entityId && response.BalanceDocument > decimal.Zero)
            .ToListAsync();
    }

    public async Task<List<AppMobileApiMGetDataDownloadDocumentCreditResponse>> PosMeFilterDocumentNumber(string filter)
    {
        return await dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditResponse>()
            .Where(response => response.DocumentNumber!.Contains(filter))
            .ToListAsync();
    }

    public async Task<AppMobileApiMGetDataDownloadDocumentCreditResponse> PosMeFindDocumentNumber(string filter)
    {
        return await dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditResponse>()
            .Where(response => response.DocumentNumber == filter)
            .FirstOrDefaultAsync();
    }
}