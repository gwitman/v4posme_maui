using SQLite;

namespace Posme.Maui.Models;

[Table("tb_transaction_master_detail")]
public class TbTransactionMasterDetail
{
    [PrimaryKey, AutoIncrement] public int TransactionMasterDetailId { get; set; }
    public string TransactionMasterId { get; set; }
    public int Componentid { get; set; }
    public int ComponentItemid { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitaryCost { get; set; }
    public decimal UnitaryPrice { get; set; }
    public decimal SubAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax1 { get; set; }
    public decimal Amount { get; set; }
    public string Reference1 { get; set; } = string.Empty;
    public string Reference2 { get; set; } = string.Empty;
}