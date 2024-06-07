using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryDocumentCreditAmortization : IRepositoryFacade<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>
{
    Task<int> PosMeCountByDocumentNumber(string document);
    Task<List<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByCustomerNumber(string filter);
    Task<List<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByDocumentNumber(string document);
    Task<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse> PosMeFindByDocumentNumber(string document);
}