using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posme.Maui.Services.Helpers
{
    public static class Constantes
    {
        public static int CompanyId = 2;
        public const string UrlBase = "https://posme.net";
        public static string UrlRequestLogin = UrlBase + "/v4posme/{CompanyKey}/public/core_acount/loginMobile";
        public static string UrlRequestDownload;

    }
}
