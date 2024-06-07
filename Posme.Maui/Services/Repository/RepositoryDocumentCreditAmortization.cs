using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryDocumentCreditAmortization(DataBase dataBase) : RepositoryFacade<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>(dataBase), IRepositoryDocumentCreditAmortization
{
    private readonly DataBase _dataBase = dataBase;

    public async Task<int> PosMeCountByDocumentNumber(string document)
    {
        return await _dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>()
            .CountAsync(response => response.DocumentNumber!.Contains(document));
    }

    public async Task<List<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByCustomerNumber(string filter)
    {
        return await _dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>()
            .Where(response => response.CustomerNumber == filter && response.Remaining > decimal.Zero)
            .OrderBy(response => response.DateApply)
            .ToListAsync();
    }

    public async Task<List<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>> PosMeFilterByDocumentNumber(string document)
    {
        var query = """
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
                    where dca.DocumentNumber = ?
                    """;
        return await _dataBase.Database.QueryAsync<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>(query, document);
    }

    public async Task<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse> PosMeFindByDocumentNumber(string document)
    {
        return await _dataBase.Database.Table<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse>()
            .FirstOrDefaultAsync(response => response.DocumentNumber == document);
    }
}