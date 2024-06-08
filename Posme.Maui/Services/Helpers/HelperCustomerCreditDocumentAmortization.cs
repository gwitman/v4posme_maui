using Posme.Maui.Services.Repository;
using System.Runtime.Intrinsics.Arm;
using Posme.Maui.Models;
using System.Reflection.Metadata;
using Unity;
namespace Posme.Maui.Services.Helpers;

class HelperCustomerCreditDocumentAmortization
{
    public async Task<string> ApplyShare(int entityId,string invoiceNumber,decimal amountApply)
    {

        IRepositoryDocumentCredit _repositoryDocumentCredit;
        IRepositoryDocumentCreditAmortization _repositoryDocumentCreditAmortization;
        IRepositoryTbCustomer _repositoryTbCustomer;

        _repositoryDocumentCredit               = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();
        _repositoryDocumentCreditAmortization   = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCreditAmortization>();
        _repositoryTbCustomer                   = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();


        string resultado = "";
        
        var objCustomDocumentAmortization   = await _repositoryDocumentCreditAmortization.PosMeFilterByDocumentNumber(invoiceNumber);
        var objCustomerDocument             = await _repositoryDocumentCredit.PosMeFindDocumentNumber(invoiceNumber);
        var objCustomerResponse             = await _repositoryTbCustomer.PosMeFindEntityID(entityId);


        var tmpListaSave        = new List<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse>();
        var amountApplyBackup   = amountApply; 

        //Actualiar Tabla de Amortiation
        foreach (var documentCreditAmortization in objCustomDocumentAmortization)
        {
            if (decimal.Compare(amountApply, decimal.Zero) <= 0)
            {
                break;
            }

            if (decimal.Compare(documentCreditAmortization.Remaining, amountApply) <= 0)
            {
                amountApply = decimal.Subtract(amountApply, documentCreditAmortization.Remaining);
                documentCreditAmortization.Remaining = decimal.Zero;
            }
            else
            {
                documentCreditAmortization.Remaining = amountApply;
                amountApply = decimal.Zero;
            }
            tmpListaSave.Add(documentCreditAmortization);
        }

        //Actualizar Documento
        objCustomerDocument.Balance = objCustomerDocument.Balance - amountApplyBackup;


        //Actulizar Saldo del Cliente 
        if (objCustomerResponse.CustomerId == objCustomerDocument.CurrencyId)
        {
            objCustomerResponse.Balance = objCustomerResponse.Balance - amountApplyBackup;
        }
        //Actualiar Saldo del cliente Linea de Credito en Dolares y Documento esta en cordoba
        else if (objCustomerDocument.CurrencyId == (int)TypeCurrency.Cordoba)
        {
            objCustomerResponse.Balance = objCustomerResponse.Balance - (amountApplyBackup / objCustomerDocument.ExchangeRate);
        }
        //Actualiar Saldo del cliente Linea de Credito en Cordoba y Documento esta en Dolares
        else if (objCustomerDocument.CurrencyId == (int)TypeCurrency.Dolar)
        {
            objCustomerResponse.Balance = objCustomerResponse.Balance - (amountApplyBackup * objCustomerDocument.ExchangeRate );
        }


        var taskAmortization = _repositoryDocumentCreditAmortization.PosMeUpdateAll(tmpListaSave);
        var taskDocument = _repositoryDocumentCredit.PosMeUpdate(objCustomerDocument);
        var taskCustomer = _repositoryTbCustomer.PosMeUpdate(objCustomerResponse);
        await Task.WhenAll([taskAmortization, taskDocument, taskCustomer]);

        return resultado;
    }

}
