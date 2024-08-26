﻿using System.Runtime.Serialization;
using Posme.Maui.Services.Helpers;
using SQLite;
using Posme.Maui.Services.SystemNames;

namespace Posme.Maui.Models;

[Table("tb_transaction_master")]
public class TbTransactionMaster
{
    [DataMember(Name = "transactionID")] public TypeTransaction TransactionId { get; set; }
    [DataMember(Name = "typePaymentID")] public TypePayment TypePaymentId { get; set; } = TypePayment.Efectivo;

    [DataMember(Name = "transactionMasterID")]
    [PrimaryKey, AutoIncrement]
    public int TransactionMasterId { get; set; }

    [DataMember(Name = "transactionNumber")]
    public string? TransactionNumber { get; set; }

    [DataMember(Name = "entityID")] public int EntityId { get; set; }
    [DataMember(Name = "transactionOn")] public DateTime TransactionOn { get; set; }

    [DataMember(Name = "entitySecondaryID")]
    public string? EntitySecondaryId { get; set; }

    [DataMember(Name = "subAmount")] public decimal SubAmount { get; set; }
    [DataMember(Name = "discount")] public decimal Discount { get; set; }
    [DataMember(Name = "tax1")] public decimal Taxi1 { get; set; }
    [DataMember(Name = "amount")] public decimal Amount { get; set; }

    [DataMember(Name = "customerCreditLineID")]
    public int CustomerCreditLineId { get; set; }

    [DataMember(Name = "transactionCausalID")]
    public TypeTransactionCausal TransactionCausalId { get; set; }

    [DataMember(Name = "exchangeRate")] public decimal ExchangeRate { get; set; }
    [DataMember(Name = "currencyID")] public TypeCurrency CurrencyId { get; set; }

    [DataMember(Name = "comment")] public string? Comment { get; set; } = string.Empty;

    [DataMember(Name = "referencie1")] public string? Reference1 { get; set; } = string.Empty;
    [DataMember(Name = "referencie2")] public string? Reference2 { get; set; } = string.Empty;
    [DataMember(Name = "referencie3")] public string? Reference3 { get; set; } = string.Empty;

    [DataMember(Name = "referencie1")] public string CustomerIdentification { get; set; } = string.Empty;
}