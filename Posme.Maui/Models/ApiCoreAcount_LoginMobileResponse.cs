﻿using SQLite;
using System.ComponentModel.DataAnnotations;

#nullable enable


namespace Posme.Maui.Models;

public class ApiCoreAcount_LoginMobileResponse
{
    public bool Error { get; set; }
    public string? Message { get; set; }
    public CoreAccountMLoginMobileObjUserResponse? ObjUser { get; set; }
}

[Table("tb_user")]
public class CoreAccountMLoginMobileObjUserResponse
{
    public int? CompanyId { get; set; }

    public int? BranchId { get; set; }

    [PrimaryKey] public int UserId { get; set; }

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

    public string? Company { get; set; }
}