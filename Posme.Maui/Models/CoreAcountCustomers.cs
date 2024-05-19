#nullable enable
using SQLite;

namespace Posme.Maui.Models;

[Table("tb_customers")]
public class CoreAcountCustomers
{
    [PrimaryKey, AutoIncrement] public int CustomerId { get; set; }
    
    public int ComapnyId { get; set; }

    public int BranchId { get; set; }

    public int EntityId { get; set; }

    public string? CustomerNumber { get; set; }

    public string? Identification { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}