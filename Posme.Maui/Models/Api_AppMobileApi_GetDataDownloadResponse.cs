using System.Runtime.Serialization;
using DevExpress.Maui.Core;
using SQLite;

namespace Posme.Maui.Models;

public class Api_AppMobileApi_GetDataDownloadResponse
{
    public bool Error { get; set; }
    public string? Message { get; set; }
    public List<Api_AppMobileApi_GetDataDownloadItemsResponse> ListItem { get; set; } = [];
    public List<Api_AppMobileApi_GetDataDownloadCustomerResponse> ListCustomer { get; set; } = [];
    public List<Api_AppMobileApi_GetDataDownloadParametersResponse> ListParameter { get; set; } = [];
    public List<Api_AppMobileApi_GetDataDownloadDocumentCreditResponse> ListDocumentCredit { get; set; } = [];

    public List<Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse> ListDocumentCreditAmortization { get; set; } = [];
}

[Table("tb_customers")]
public class Api_AppMobileApi_GetDataDownloadCustomerResponse : BindableBase
{
    [PrimaryKey, AutoIncrement]
    [DataMember]
    public int CustomerId { get; set; }

    [DataMember] public int ComapnyId { get; set; }

    [DataMember] public int BranchId { get; set; }

    [DataMember] public int EntityId { get; set; }

    [DataMember] public string? CustomerNumber { get; set; }

    [DataMember] public string? Identification { get; set; }

    [DataMember] public string? FirstName { get; set; }

    [DataMember] public string? LastName { get; set; }

    [DataMember] public decimal Balance { get; set; }
    
    [DataMember] public int CurrencyId { get; set; }
    [DataMember] public int CustomerCreditLineId { get; set; }
    public bool Modificado { get; set; }
}

[Table("tb_document_credit")]
public class Api_AppMobileApi_GetDataDownloadDocumentCreditResponse
{
    [PrimaryKey, AutoIncrement] public int CustomerCreditDocumentId { get; set; }
    public int EntityId { get; set; }
    public int CustomerCreditLineId { get; set; }
    public string? DocumentNumber { get; set; }
    public string? CurrencyName { get; set; }
    public int CurrencyId { get; set; }
    public int StatusDocument { get; set; }
    public int CreditAmortizationId { get; set; }
    public DateTime DateApply { get; set; }
    public decimal Remaining { get; set; }
    public int StatusAmortization { get; set; }
    public string? StatusAmortizationName { get; set; }
    public decimal Balance { get; set; }
    public decimal ExchangeRate { get; set; }
}

[Table("document_credit_amortization")]
public class Api_AppMobileApi_GetDataDownloadDocumentCreditAmortizationResponse
{
    [PrimaryKey,AutoIncrement] public int CreditAmortizationID { get; set; }    
    public string? CustomerNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string? DocumentNumber { get; set; }

    public int CurrencyId { get; set; }

    public string? ReportSinRiesgo { get; set; }

    public DateTime DateApply { get; set; }

    public decimal Remaining { get; set; }

    public decimal Balance { get; set; }
}

[SQLite.Table("tb_items")]
public class Api_AppMobileApi_GetDataDownloadItemsResponse : BindableBase
{
    [PrimaryKey, AutoIncrement]
    [DataMember]
    public int ItemPk
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    [DataMember]
    public int ItemId
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    [DataMember]
    public string BarCode
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    [DataMember]
    public string? ItemNumber
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    [DataMember]
    public string Name
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    [DataMember]
    public decimal Quantity
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    [DataMember]
    public decimal PrecioPublico
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public decimal CantidadEntradas
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public decimal CantidadSalidas
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public decimal CantidadFinal
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public bool Modificado { get; set; }
}

[SQLite.Table("tb_parameters")]
public class Api_AppMobileApi_GetDataDownloadParametersResponse
{
    [PrimaryKey, AutoIncrement] public int ParametersPk { get; set; }

    public int ComapnyId { get; set; }

    public int ParameterId { get; set; }

    public string? Display { get; set; }

    public string? Description { get; set; }

    public string? Value { get; set; }

    public string? CustomValue { get; set; }

    public string? Name { get; set; }
}