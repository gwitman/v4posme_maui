﻿using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Unity;

namespace Posme.Maui.Services.Api;

public class RestApiAppMobileApi
{
    private readonly HttpClient _httpClient = new();

    private readonly IRepositoryTbCustomer _repositoryTbCustomer =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();

    private readonly IRepositoryItems _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();

    private readonly IRepositoryParameters _repositoryParameters =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryParameters>();

    private readonly IRepositoryDocumentCreditAmortization _repositoryDocumentCreditAmortization =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCreditAmortization>();

    private readonly IRepositoryDocumentCredit _repositoryDocumentCredit =
        VariablesGlobales.UnityContainer.Resolve<IRepositoryDocumentCredit>();

    private readonly IRepositoryTbCompany _repositoryTbCompany = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCompany>();

    private readonly IRepositoryTbTransactionMaster _repositoryTbTransactionMaster = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMaster>();
    private readonly IRepositoryTbTransactionMasterDetail _repositoryTbTransactionMasterDetail = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMasterDetail>();

    public async Task<bool> GetDataDownload()
    {
        var helper = VariablesGlobales.UnityContainer.Resolve<HelperCore>();
        Constantes.UrlRequestDownload = Constantes.UrlRequestDownload.Replace("{CompanyKey}", VariablesGlobales.CompanyKey);
        Constantes.UrlRequestDownload = await helper.ParseUrl(Constantes.UrlRequestDownload);

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
            var companyDeleteAll = _repositoryTbCompany.PosMeDeleteAll();
            await Task.WhenAll([
                customerDeleteAll, itemsDeleteAll, documentCreditAmortizationDeleteAll, parametersDeleteAll,
                documentCreditDeleteAll, companyDeleteAll
            ]);

            var taskCompany = _repositoryTbCompany.PosMeInsert(apiResponse.ObjCompany);
            var taskCustomer = _repositoryTbCustomer!.PosMeInsertAll(apiResponse.ListCustomer);
            var taskItem = _repositoryItems!.PosMeInsertAll(apiResponse.ListItem);
            var taskDocumentCreditAmortization =
                _repositoryDocumentCreditAmortization!.PosMeInsertAll(apiResponse.ListDocumentCreditAmortization);
            var taskParameters = _repositoryParameters!.PosMeInsertAll(apiResponse.ListParameter);
            var taskDocumentCredit = _repositoryDocumentCredit!.PosMeInsertAll(apiResponse.ListDocumentCredit);
            await Task.WhenAll([
                taskCustomer, taskItem, taskDocumentCreditAmortization, taskParameters,
                taskDocumentCredit, taskCompany
            ]);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }

    public async Task<string> SendDataAsync()
    {
        try
        {
            
            var nickname = VariablesGlobales.User!.Nickname!;
            var password = VariablesGlobales.User.Password!;
            var helper = VariablesGlobales.UnityContainer.Resolve<HelperCore>();
            var findCustomers = await _repositoryTbCustomer.PosMeTakeModificados();
            var findItems = await _repositoryItems.PosMeTakeModificado();
            var findTransactionMaster = await _repositoryTbTransactionMaster.PosMeFindAll();
            var findTransactionMasterDetail = await _repositoryTbTransactionMasterDetail.PosMeFindAll();
            var data = new Dictionary<string, object>
            {
                { "ObjCustomers", findCustomers },
                { "ObjItems", findItems },
                { "ObjTransactionMaster", findTransactionMaster },
                { "ObjTransactionMasterDetail", findTransactionMasterDetail }
            };
            var jsonData = JsonConvert.SerializeObject(data);
            var nvc = new List<KeyValuePair<string, string>>
            {
                new("txtNickname", nickname),
                new("txtPassword", password),
                new("txtData", jsonData)
            };
            var content = new FormUrlEncodedContent(nvc);

            Constantes.UrlUpload = Constantes.UrlUpload.Replace("{CompanyKey}", VariablesGlobales.CompanyKey);
            Constantes.UrlUpload = await helper.ParseUrl(Constantes.UrlUpload);
            var req = new HttpRequestMessage(HttpMethod.Post, Constantes.UrlUpload)
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(req);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}