using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Unity;

namespace Posme.Maui.Services;

public class RealizarPagos
{
    private readonly IRepositoryTbCompany _repositoryTbCompany = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCompany>();
    private readonly IRepositoryParameters _repositoryParameters = VariablesGlobales.UnityContainer.Resolve<IRepositoryParameters>();
    public string Mensaje { get; private set; } = string.Empty;

    public async Task<bool> GenerarUrl(List<Api_AppMobileApi_GetDataDownloadItemsResponse> itemsResponses, TbTransactionMaster transactionMaster)
    {
        try
        {
            var company = await _repositoryTbCompany.PosMeFindFirst();
            var client = new HttpClient();
            var details = new Dictionary<string, object>();
            var listaDetails = new List<object>();
            var username = await _repositoryParameters.PosMeFindByKey("CORE_PAYMENT_PRODUCCION_USUARIO");
            var password = await _repositoryParameters.PosMeFindByKey("CORE_PAYMENT_PRODUCCION_CLAVE");
            var authToken = Encoding.UTF8.GetBytes($"{username!.Value}:{password!.Value}");
            foreach (var item in itemsResponses)
            {
                details.Add("quantity", item.Quantity);
                details.Add("description", item.Name);
                details.Add("price", item.PrecioPublico);
                listaDetails.Add(details);
            }

            var customParam = new Dictionary<string, string>();

            var simboloMoneda = transactionMaster.CurrencyId switch
            {
                TypeCurrency.Cordoba => Mensajes.MonedaCordoba,
                TypeCurrency.Dolar => Mensajes.MonedaDolar,
                _ => ""
            };
            var token = Convert.ToBase64String(authToken);
            var ern = $"{VariablesGlobales.CompanyKey}_{DateTime.Now:yyyyMMddHHmmss}";
            var data = new Dictionary<string, object>
            {
                { "ern", ern },
                { "amount", transactionMaster.Amount },
                { "currency", simboloMoneda },
                { "extended_expiration", false },
                { "details", listaDetails }
            };
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Constantes.UrlPagadito),
                Content = content
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Mensaje = response.StatusCode.ToString();
                return false;
            }

            Mensaje = await response.Content.ReadAsStringAsync();
            return true;
        }
        catch (Exception e)
        {
            Mensaje = e.Message;
            return false;
        }
    }

    public async Task<string> ExecuteTransactionAsync()
    {
        var httpClient = new HttpClient();
        var url = "https://connect.pagadito.com/api/v2/exec-trans";

        var content = new
        {
            ern = "PG-0001",
            amount = 10,
            currency = "USD",
            extended_expiration = false,
            details = new[]
            {
                new
                {
                    quantity = 1,
                    description = "Product 1",
                    price = 10,
                    url_product = "http://www.example.com/product1"
                }
            },
            custom_params = new
            {
                param1 = "value1",
                param2 = "value2",
                param3 = "value3"
            }
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = jsonContent
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "N2J1NjI1NWI2OWVkNmNhYzQ5ZGUyOWJkMzZmNDY2NWU6MTJlZjA0ZTcxOTdjMDAxYjY2OTIwNzY3ZmFjNjNjNDY=");
        request.Headers.Add("X-CSRF-TOKEN", string.Empty);

        try
        {
            var response = await httpClient.SendAsync(request);
            var resultContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
                Debug.WriteLine($"Response: {resultContent}");
                return resultContent;
            }

            Debug.WriteLine(resultContent);
            return resultContent;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex.Message}");
            return ex.Message;
        }
    }
}