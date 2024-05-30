﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posme.Maui.Services.Helpers;

public static class Constantes
{
    public static string ParametroCounter = "COUNTER";
    public static string ParametroLogo = "LOGO";
    public static string ParametroAccesPoint = "ACCESS_POINT";
    public static string ParametroPrinter = "PRINTER";
    public static int CompanyId = 2;
    public const string UrlBase = "https://posme.net";
    public static string UrlRequestLogin = UrlBase + "/v4posme/{CompanyKey}/public/core_acount/loginMobile";
    public static string UrlRequestDownload = UrlBase + "/v4posme/{CompanyKey}/public/app_mobile_api/getDataDownload";
}