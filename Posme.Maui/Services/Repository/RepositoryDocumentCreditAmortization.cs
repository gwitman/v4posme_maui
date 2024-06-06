using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryDocumentCreditAmortization(DataBase dataBase) : RepositoryFacade<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>(dataBase), IRepositoryDocumentCreditAmortization
{
    public async Task<int> PosMeCountByDocumentNumber(string document)
    {
        return await dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>()
            .CountAsync(response => response.DocumentNumber!.Contains(document));
    }

    public async Task<List<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByDocumentNumber(string document)
    {
        var query = $"""
                     select tdc.CurrencyName,
                            dca.documentcreditamortizationpk,
                            dca.customernumber,
                            dca.firstname,
                            dca.lastname,
                            dca.birthdate,
                            dca.documentnumber,
                            dca.currencyid,
                            dca.reportsinriesgo,
                            dca.dateapply,
                            dca.remaining,
                            dca.sharecapital,
                            dca.currencyname
                     from document_credit_amortization dca
                              join main.tb_document_credit tdc on dca.DocumentNumber = tdc.DocumentNumber
                     where dca.DocumentNumber = '{document}'
                     """;
        return await dataBase.Database.QueryAsync<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>(query);
    }

    public async Task<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse> PosMeFindByDocumentNumber(string document)
    {
        return await dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>()
            .FirstOrDefaultAsync(response => response.DocumentNumber == document);
    }
}