using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posme.Maui.Services.SystemNames;

public static class Constantes
{
    public static readonly string ParametroCounter = "COUNTER";
    public static readonly string ParametroLogo = "LOGO";
    public static readonly string ParametroAccesPoint = "ACCESS_POINT";
    public static readonly string ParametroPrinter = "PRINTER";
    public static readonly string ParametroCodigoAbono = "TRANSACTION_SHARE";
    public static int CompanyId = 2;
    public const string UrlBase = "https://posme.net/v4posme/";
    public const string ParameterCodigoFactura = "TRANSACTION_INVOICE";
    public static string UrlRequestLogin = UrlBase + "{CompanyKey}/public/core_acount/loginMobile";
    public static string UrlRequestDownload = UrlBase + "{CompanyKey}/public/app_mobile_api/getDataDownload";
    public static string UrlUpload = UrlBase + "{CompanyKey}/public/app_mobile_api/setDataUpload";
    //public const string UrlPagadito = "https://connect.pagadito.com/api/v2/exec-trans";
    public const string UrlPagadito = "https://sandbox-connect.pagadito.com/connect/api/v2/exec-trans";
}