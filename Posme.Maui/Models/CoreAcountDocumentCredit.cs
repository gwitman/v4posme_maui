using SQLite;

namespace Posme.Maui.Models;

[Table("tb_document_credit")]
public class CoreAcountDocumentCredit
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