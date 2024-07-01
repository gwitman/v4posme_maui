using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posme.Maui.Services.SystemNames
{
    public enum TypeTransaction
    {
        TransactionInvoiceBilling = 19,
        TransactionShare = 25
    }

    public enum TypeCurrency
    {
        Cordoba = 1,
        Dolar = 2,
    }
    public enum TypeTransactionCausal {
        Contado = 21,
        Credito = 22
    }
    public enum  TypeComponent
    {
        Itme = 33,
        Customer = 36
    }

    public enum TypePayment
    {
        TarjetaDebito=1,
        TarjetaCredito,
        Efectivo,
        Cheque,
        Monedero,
        Otros
    }
}
