using System.Diagnostics;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Posme.Maui.Models;

namespace Posme.Maui.Services.Helpers;

public class RestApiCoreAcountMLogin
{
    private readonly HttpClient _httpClient = new();
    private readonly DataBase _dataBase = new();

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
            var apiResponse = JsonConvert.DeserializeObject<CoreAcountMLoginMobileResponse>(responseBody);
            if (apiResponse.ObjUser is null) return false;
            VariablesGlobales.User = apiResponse.ObjUser;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }

    public async Task InsertUser(CoreAccountMLoginMobileObjUserResponse objUser)
    {
        await _dataBase.Database.InsertAsync(objUser);
    }

    public async Task UpdateUser(CoreAccountMLoginMobileObjUserResponse objUser)
    {
        await _dataBase.Database.UpdateAsync(objUser);
    }
    public async Task OnRemember()
    {
        var listAsync = await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>().ToListAsync();
        if (listAsync.Count<=0)
        {
            return;
        }
        
        foreach (var user in listAsync)
        {
            user.Remember = false;
        }
        
        await _dataBase.Database.UpdateAllAsync(listAsync);
    }

    public async Task<CoreAccountMLoginMobileObjUserResponse> FindUserRemember()
    {
        return await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>()
            .Where(user => user.Remember)
            .FirstOrDefaultAsync();
    }

    public async Task<CoreAccountMLoginMobileObjUserResponse> FindUserByNicknameAndPassword(string nickname, string password)
    {
        return await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>()
            .Where(user => user.Nickname==nickname && user.Password==password)
            .FirstOrDefaultAsync();
    }
    
    public async Task<int> RowCount()
    {
        return await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>().CountAsync();
    }
}