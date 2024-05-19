using System.Diagnostics;
using Newtonsoft.Json;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;

namespace Posme.Maui.Services.Helpers;

public class RestApiAppMobileApi(IServiceProvider services)
{
    private readonly HttpClient _httpClient = new();
    private readonly IRepositoryTbCustomer? _repositoryTbCustomer = services.GetService<IRepositoryTbCustomer>();
    private readonly IRepositoryItems? _repositoryItems = services.GetService<IRepositoryItems>();
    private readonly IRepositoryParameters? _repositoryParameters = services.GetService<IRepositoryParameters>();
    private readonly IRepositoryDocumentCreditAmortization? _repositoryDocumentCreditAmortization = services.GetService<IRepositoryDocumentCreditAmortization>();
    private readonly IRepositoryDocumentCredit? _repositoryDocumentCredit = services.GetService<IRepositoryDocumentCredit>();

    public async Task<bool> GetDataDownload()
    {
        Constantes.UrlRequestDownload = Constantes.UrlRequestDownload.Replace("{CompanyKey}", VariablesGlobales.CompanyKey);
        if (VariablesGlobales.User is null)
        {
            return false;
        }
        try
        {
            var nickname = VariablesGlobales.User.Nickname!;
            var password = VariablesGlobales.User.Password!;
            var nvc = new List<KeyValuePair<string, string>>
            {
                new("txtNickname", nickname),
                new("txtPassword", password)
            };
            var req = new HttpRequestMessage(HttpMethod.Post, Constantes.UrlRequestDownload)
            {
                Content = new FormUrlEncodedContent(nvc)
            };

            var response = await _httpClient.SendAsync(req);
            if (!response.IsSuccessStatusCode) return false;
            var responseBody = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<AppMobileApiMGetDataDownloadResponse>(responseBody);
            if (apiResponse is null || apiResponse.Error) return false;

            var taskCustomer= _repositoryTbCustomer!.PosMeInsertAll(apiResponse.ListCustomer);
            var taskItem= _repositoryItems!.PosMeInsertAll(apiResponse.ListItem);
            var taskDocumentCreditAmortization= _repositoryDocumentCreditAmortization!.PosMeInsertAll(apiResponse.ListDocumentCreditAmortization);
            var taskParameters= _repositoryParameters!.PosMeInsertAll(apiResponse.ListParameter);
            var taskDocumentCredit= _repositoryDocumentCredit!.PosMeInsertAll(apiResponse.ListDocumentCredit);
            await Task.WhenAll([taskCustomer, taskItem, taskDocumentCreditAmortization, taskParameters, taskDocumentCredit]);
            return true;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }
}