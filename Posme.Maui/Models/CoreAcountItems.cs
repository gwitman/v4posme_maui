#nullable enable
using SQLite;

namespace Posme.Maui.Models;

[Table("tb_items")]
public class CoreAcountItems
{
    [PrimaryKey, AutoIncrement] public int ItemPk { get; set; }
    
    public int ItemId { get; set; }

    public string? BarCode { get; set; }

    public string? ItemNumber { get; set; }

    public string? Name { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? PrecioPublico { get; set; }
}