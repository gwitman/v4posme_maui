using Posme.Maui.Services.Helpers;
using SQLite;
using Posme.Maui.Services.SystemNames;
namespace Posme.Maui.Models;

[Table("tb_transaction_master")]
public class TbTransactionMaster
{
    public TypeTransaction TransactionId { get; set; }
    [PrimaryKey, AutoIncrement] public int TransactionMasterId { get; set; }
    public string? TransactionNumber { get; set; }
    public int EntityId { get; set; }
    public DateTime TransactionOn { get; set; }
    public string? EntitySecondaryId { get; set; }
    public decimal SubAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal Taxi1 { get; set; }
    public decimal Amount { get; set; }
    public int TransactionCausalId { get; set; }
    public decimal ExchangeRate { get; set; }    
    public int CurrencyId { get; set; }
    public string? Comment { get; set; }
    public string? Reference1 { get; set; }
    public string? Reference2 { get; set; }
    public string? Reference3 { get; set; }
}