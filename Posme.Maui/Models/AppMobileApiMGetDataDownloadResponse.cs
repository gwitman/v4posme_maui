﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DevExpress.Maui.Core;
using SQLite;

namespace Posme.Maui.Models;

public class AppMobileApiMGetDataDownloadResponse
{
    public bool Error { get; set; }
    public string? Message { get; set; }
    public List<AppMobileApiMGetDataDownloadItemsResponse> ListItem { get; set; } = [];
    public List<AppMobileApiMGetDataDownloadCustomerResponse> ListCustomer { get; set; } = [];
    public List<AppMobileApiMGetDataDownloadParametersResponse> ListParameter { get; set; } = [];
    public List<AppMobileApiMGetDataDownloadDocumentCreditResponse> ListDocumentCredit { get; set; } = [];

    public List<AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse> ListDocumentCreditAmortization
    {
        get;
        set;
    } = [];
}

[Table("tb_customers")]
public class AppMobileApiMGetDataDownloadCustomerResponse : BindableBase
{
    [PrimaryKey, AutoIncrement] [DataMember]public int CustomerId { get; set; }

    [DataMember]public int ComapnyId { get; set; }

    [DataMember]public int BranchId { get; set; }

    [DataMember]public int EntityId { get; set; }

    [DataMember]public string? CustomerNumber { get; set; }

    [DataMember]public string? Identification { get; set; }

    [DataMember]public string? FirstName { get; set; }

    [DataMember]public string? LastName { get; set; }

    [DataMember]public decimal Balance { get; set; }
    public bool Modificado { get; set; }
}

[Table("tb_document_credit")]
public class AppMobileApiMGetDataDownloadDocumentCreditResponse
{
    [PrimaryKey, AutoIncrement] public int DocumentCreditPk { get; set; }

    public int EntityId { get; set; }

    public int CustomerCreditDocumentId { get; set; }

    public string? DocumentNumber { get; set; }

    public decimal BalanceDocument { get; set; }

    public int CurrencyId { get; set; }

    public int StatusDocument { get; set; }

    public int CreditAmortizationId { get; set; }

    public DateTime DateApply { get; set; }

    public decimal Remaingin { get; set; }

    public int StatusAmortization { get; set; }

    public string? StatusAmortizationName { get; set; }
}

[Table("document_credit_amortization")]
public class AppMobileApiMGetDataDownloadDocumentCreditAmortizationResponse
{
    [PrimaryKey, AutoIncrement] public int DocumentCreditAmortizationPk { get; set; }

    public string? CustomerNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string? DocumentNumber { get; set; }

    public int CurrencyId { get; set; }

    public string? ReportSinRiesgo { get; set; }

    public DateTime DateApply { get; set; }

    public decimal Remaining { get; set; }

    public decimal ShareCapital { get; set; }
}

[Table("tb_items")]
public class AppMobileApiMGetDataDownloadItemsResponse : BindableBase
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
    [Required(ErrorMessage = "Required")]
    public string BarCode
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    [DataMember]
    [Required(ErrorMessage = "Required")]
    public string? ItemNumber
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    [DataMember]
    [Required(ErrorMessage = "Debe especificar una descripción del producto")]
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

    public decimal CantidadEntradas {
        get => GetValue<decimal>();
        set => SetValue(value);
    }
    public decimal CantidadSalidas {
        get => GetValue<decimal>();
        set => SetValue(value);
    }
    public decimal CantidadFinal {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public bool Modificado { get; set; }
}

[Table("tb_parameters")]
public class AppMobileApiMGetDataDownloadParametersResponse
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