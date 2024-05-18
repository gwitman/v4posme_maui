using SQLite;
using System.ComponentModel.DataAnnotations;

#nullable enable


namespace Posme.Maui.Models;

public class CoreAcountMLoginMobileResponse
{
    public bool Error { get; set; }
    public string? Message { get; set; }
    public CoreAccountMLoginMobileObjUserResponse? ObjUser { get; set; }
}

[Table("tb_user")]
public class CoreAccountMLoginMobileObjUserResponse
{
    public int? CompanyID { get; set; }

    public int? BranchID { get; set; }

    [PrimaryKey] public int UserID { get; set; }

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
    [Column("token_google_calendar")]
    public string? TokenGoogleCalendar { get; set; }

    [Column("remember")] public bool Remember { get; set; }

    [Column("company")] public string? Company { get; set; }
}