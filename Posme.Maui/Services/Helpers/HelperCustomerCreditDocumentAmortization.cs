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
        var repositoryDocumentCredit = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();
        var repositoryDocumentCreditAmortization = VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCreditAmortization>();
        var repositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();


        string resultado = "";
        
        var objCustomDocumentAmortization   = await repositoryDocumentCreditAmortization.PosMeFilterByDocumentNumber(invoiceNumber);
        var objCustomerDocument             = await repositoryDocumentCredit.PosMeFindDocumentNumber(invoiceNumber);
        var objCustomerResponse             = await repositoryTbCustomer.PosMeFindEntityId(entityId);


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
                documentCreditAmortization.Remaining = decimal.Subtract(documentCreditAmortization.Remaining, amountApply);
                amountApply = decimal.Zero;
            }
            tmpListaSave.Add(documentCreditAmortization);
        }

        //Actualizar Documento
        objCustomerDocument.Balance -= amountApplyBackup;


        //Actulizar Saldo del Cliente 
        if (objCustomerResponse.CurrencyId == objCustomerDocument.CurrencyId)
        {
            objCustomerResponse.Balance -= amountApplyBackup;
        }
        //Actualiar Saldo del cliente Linea de Credito en Dolares y Documento esta en cordoba
        else if (objCustomerDocument.CurrencyId == (int)TypeCurrency.Cordoba && objCustomerDocument.CurrencyId==(int)TypeCurrency.Dolar)
        {
            objCustomerResponse.Balance -= decimal.Round(amountApplyBackup / objCustomerDocument.ExchangeRate,2);
        }
        //Actualiar Saldo del cliente Linea de Credito en Cordoba y Documento esta en Dolares
        else if (objCustomerDocument.CurrencyId == (int)TypeCurrency.Dolar && objCustomerDocument.CurrencyId == (int)TypeCurrency.Cordoba)
        {
            objCustomerResponse.Balance -= decimal.Round(amountApplyBackup * objCustomerDocument.ExchangeRate,2);
        }


        var taskAmortization = repositoryDocumentCreditAmortization.PosMeUpdateAll(tmpListaSave);
        var taskDocument = repositoryDocumentCredit.PosMeUpdate(objCustomerDocument);
        var taskCustomer = repositoryTbCustomer.PosMeUpdate(objCustomerResponse);
        await Task.WhenAll([taskAmortization, taskDocument, taskCustomer]);

        return resultado;
    }

}
