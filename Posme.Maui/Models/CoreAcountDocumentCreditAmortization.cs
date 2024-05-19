using SQLite;

namespace Posme.Maui.Models;

[Table("document_credit_amortization")]
public class CoreAcountDocumentCreditAmortization
{
    [PrimaryKey,AutoIncrement] public int DocumentCreditAmortizationPk { get; set; }
    
    public string? CustomerNumber { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
     public string? DocumentNumber { get; set; }
    
    public int CurrencyId { get; set; }
    
    public string? ReportSinRiesgo { get; set;}
    
    public DateTime DateApply { get; set; }
    
    public decimal Remaining { get; set; }
    
    public decimal ShareCapital { get; set; }
}