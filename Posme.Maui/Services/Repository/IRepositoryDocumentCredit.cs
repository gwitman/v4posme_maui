using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryDocumentCredit : IRepositoryFacade<AppMobileApiMGetDataDownloadDocumentCreditResponse>
{
    Task<List<AppMobileApiMGetDataDownloadDocumentCreditResponse>> PosMeFindByEntityId(int entityId);

    Task<List<AppMobileApiMGetDataDownloadDocumentCreditResponse>> PosMeFilterDocumentNumber(string filter);
    Task<AppMobileApiMGetDataDownloadDocumentCreditResponse> PosMeFindDocumentNumber(string filter);
}