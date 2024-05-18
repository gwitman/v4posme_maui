#nullable enable
using System.ComponentModel.DataAnnotations;
using SQLite;

namespace Posme.Maui.Models;

[Table("tb_user")]
public class ObjUser
{
    public int? CompanyID { get; set; }

    public int? BranchID { get; set; }

    [PrimaryKey, AutoIncrement] 
    public int UserID { get; set; }

    [Length(maximumLength: 250, minimumLength: 5)]
    public string? Nickname { get; set; }

    [Length(maximumLength: 250, minimumLength: 5)]
    public string? Password { get; set; }

    public string? CreatedOn { get; set; }

    [Length(maximumLength: 250, minimumLength: 5)]
    public string? Email { get; set; }

    public int? CreatedBy { get; set; }
    
    public int? EmployeeId { get; set; }
    
    public int? UseMobile { get; set; }
    
    public string? Phone { get; set; }
    
    public DateTime? LastPayment { get; set; }

    [Length(maximumLength: 250, minimumLength: 5)]
    public string? Comercio { get; set; }

    [Length(maximumLength: 250, minimumLength: 5)]
    public string? Foto { get; set; }

    [Length(maximumLength: 250, minimumLength: 5)]
    public string? TokenGoogleCalendar { get; set; }
    
    public bool Remember { get; set; }
}