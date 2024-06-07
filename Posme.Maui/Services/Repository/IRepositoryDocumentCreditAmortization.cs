using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryDocumentCreditAmortization : IRepositoryFacade<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse>
{
    Task<int> PosMeCountByDocumentNumber(string document);
    Task<List<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByCustomerNumber(string filter);
    Task<List<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByDocumentNumber(string document);
    Task<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse> PosMeFindByDocumentNumber(string document);
}