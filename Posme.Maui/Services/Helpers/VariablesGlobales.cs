using Posme.Maui.Models;

namespace Posme.Maui.Services.Helpers;

public static class VariablesGlobales
{
    public static int CompanyId = 2;
    public const string UrlBase = "https://posme.net";
    public static  string CompanyKey; // la toma del text companyId
    public static string UrlRequestLogin;
    public static string UrlRequestDownload;

    public static ObjUser User;
}