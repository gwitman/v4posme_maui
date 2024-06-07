using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryDocumentCredit : IRepositoryFacade<Api_AppMobileApi_GetDataDownloadDocumentCreditResponse>
{
    Task<List<Api_AppMobileApi_GetDataDownloadDocumentCreditResponse>> PosMeFindByEntityId(int entityId);

    Task<List<Api_AppMobileApi_GetDataDownloadDocumentCreditResponse>> PosMeFilterDocumentNumber(string filter);
    Task<Api_AppMobileApi_GetDataDownloadDocumentCreditResponse> PosMeFindDocumentNumber(string filter);
}