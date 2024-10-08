﻿using System;
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
    public const string UrlBase = "{UrlBase}";
    public const string ParameterCodigoFactura = "TRANSACTION_INVOICE";
    public static string UrlRequestLogin = UrlBase + "core_acount/loginMobile";
    public static string UrlRequestDownload = UrlBase + "app_mobile_api/getDataDownload";
    public static string UrlUpload = UrlBase + "app_mobile_api/setDataUpload";
    public const string UrlPagadito = "https://connect.pagadito.com/api/v2/exec-trans";
    public const string UrlPagaditoToken = "https://comercios.pagadito.com/apipg/charges.php";
    public const string TokenPagadito = "";
    public const string DescripcionRealizarPago = "LICENCIA MOBIL";
}