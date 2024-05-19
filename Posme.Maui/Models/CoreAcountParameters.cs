using SQLite;

namespace Posme.Maui.Models;

[Table("tb_parameters")]
public class CoreAcountParameters
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