using System.Diagnostics;
using Newtonsoft.Json;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Services.Helpers;

public class RestApiAppMobileApi
{
    

    private readonly HttpClient _httpClient = new();

    private readonly IRepositoryTbCustomer? _repositoryTbCustomer =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();

    private readonly IRepositoryItems? _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();

    private readonly IRepositoryParameters? _repositoryParameters =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryParameters>();

    private readonly IRepositoryDocumentCreditAmortization? _repositoryDocumentCreditAmortization =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCreditAmortization>();

    private readonly IRepositoryDocumentCredit? _repositoryDocumentCredit =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();

    public async Task<bool> GetDataDownload()
    {
        var _helper = VariablesGlobales.UnityContainer.Resolve<Helper>();
        Constantes.UrlRequestDownload = Constantes.UrlRequestDownload.Replace("{CompanyKey}", VariablesGlobales.CompanyKey);
        Constantes.UrlRequestDownload = await _helper.ParseUrl(Constantes.UrlRequestDownload);

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
            var apiResponse = JsonConvert.DeserializeObject<Api_AppMobileApi_GetDataDownloadResponse>(responseBody);
            if (apiResponse is null || apiResponse.Error) return false;
            var customerDeleteAll = _repositoryTbCustomer!.PosMeDeleteAll();
            var itemsDeleteAll = _repositoryItems!.PosMeDeleteAll();
            var documentCreditAmortizationDeleteAll = _repositoryDocumentCreditAmortization!.PosMeDeleteAll();
            var parametersDeleteAll = _repositoryParameters!.PosMeDeleteAll();
            var documentCreditDeleteAll = _repositoryDocumentCredit!.PosMeDeleteAll();
            await Task.WhenAll([
                customerDeleteAll, itemsDeleteAll, documentCreditAmortizationDeleteAll, parametersDeleteAll,
                documentCreditDeleteAll
            ]);

            var taskCustomer = _repositoryTbCustomer!.PosMeInsertAll(apiResponse.ListCustomer);
            var taskItem = _repositoryItems!.PosMeInsertAll(apiResponse.ListItem);
            var taskDocumentCreditAmortization =
                _repositoryDocumentCreditAmortization!.PosMeInsertAll(apiResponse.ListDocumentCreditAmortization);
            var taskParameters = _repositoryParameters!.PosMeInsertAll(apiResponse.ListParameter);
            var taskDocumentCredit = _repositoryDocumentCredit!.PosMeInsertAll(apiResponse.ListDocumentCredit);
            await Task.WhenAll([
                taskCustomer, taskItem, taskDocumentCreditAmortization, taskParameters, taskDocumentCredit
            ]);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }
}