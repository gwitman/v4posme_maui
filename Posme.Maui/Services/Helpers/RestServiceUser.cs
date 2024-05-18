using System.Diagnostics;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Posme.Maui.Models;

namespace Posme.Maui.Services.Helpers;

public class RestServiceUser
{
    private readonly HttpClient _httpClient = new();
    private readonly DataBase _dataBase = DependencyService.Get<DataBase>();

    public async Task<bool> FindUser(string nickname, string password)
    {
        VariablesGlobales.UrlRequestLogin =
            $"{VariablesGlobales.UrlBase}/v4posme/{VariablesGlobales.CompanyKey}/public/core_acount/loginMobile";

        try
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("txtNickname", nickname));
            nvc.Add(new KeyValuePair<string, string>("txtPassword", password));
            var req = new HttpRequestMessage(HttpMethod.Post, VariablesGlobales.UrlRequestLogin)
            {
                Content = new FormUrlEncodedContent(nvc)
            };

            var response = await _httpClient.SendAsync(req);
            if (!response.IsSuccessStatusCode) return false;
            var responseBody = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseUser>(responseBody);
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

    public async Task InsertUser(ObjUser objUser)
    {
        if (objUser.UserID != 0)
        {
            await _dataBase.Database.UpdateAsync(objUser);
            return;
        }

        await _dataBase.Database.InsertAsync(objUser);
    }

    public async Task OnRemember()
    {
        var listAsync = await _dataBase.Database.Table<ObjUser>().ToListAsync();
        foreach (var user in listAsync)
        {
            user.Remember = false;
            await InsertUser(user);
        }
    }

    public async Task<ObjUser> FindUserRemember()
    {
        return await _dataBase.Database.Table<ObjUser>()
            .Where(user => user.Remember)
            .FirstOrDefaultAsync();
    }

    public async Task<int> RowCount()
    {
        return await _dataBase.Database.Table<ObjUser>().CountAsync();
    }
}