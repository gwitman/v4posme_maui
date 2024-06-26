﻿using System.Diagnostics;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Posme.Maui.Models;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Helpers;
namespace Posme.Maui.Services.Api;

public class RestApiCoreAcount
{
    private readonly HttpClient _httpClient = new();
    
    public async Task<bool> LoginMobile(string nickname, string password)
    {
        Constantes.UrlRequestLogin = Constantes.UrlRequestLogin.Replace("{CompanyKey}", VariablesGlobales.CompanyKey);

        try
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("txtNickname", nickname));
            nvc.Add(new KeyValuePair<string, string>("txtPassword", password));
            var req = new HttpRequestMessage(HttpMethod.Post, Constantes.UrlRequestLogin)
            {
                Content = new FormUrlEncodedContent(nvc)
            };

            var response = await _httpClient.SendAsync(req);
            if (!response.IsSuccessStatusCode) return false;
            var responseBody = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<Api_CoreAcount_LoginMobileResponse>(responseBody);
            if (apiResponse is null || apiResponse.Error) return false;
            VariablesGlobales.User = apiResponse.ObjUser;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }

    
}