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

    public async Task<Api_Pagadito_Response_Exec?> GenerarUrl(List<Api_AppMobileApi_GetDataDownloadItemsResponse> itemsResponses, TbTransactionMaster transactionMaster)
    {
        try
        {
            var company = await _repositoryTbCompany.PosMeFindFirst();
            var client = new HttpClient();

            //generar token
            var usuarioPagadito = await _repositoryParameters.PosMeFindByKey("CORE_PAYMENT_PRODUCCION_USUARIO");
            var passwordPagadito = await _repositoryParameters.PosMeFindByKey("CORE_PAYMENT_PRODUCCION_CLAVE");
            var nickname = usuarioPagadito!.Value;
            var password = passwordPagadito!.Value;
            var nvc = new List<KeyValuePair<string, string>>
            {
                new("uid", nickname!),
                new("wsk", password!),
                new("format_return", "json"),
                new("operation", "f3f191ce3326905ff4403bb05b0de150")
            };
            var req = new HttpRequestMessage(HttpMethod.Post, Constantes.UrlPagaditoToken)
            {
                Content = new FormUrlEncodedContent(nvc)
            };

            var responseTokenMessage = await client.SendAsync(req);
            if (!responseTokenMessage.IsSuccessStatusCode) return null;
            var responseBodyToken = await responseTokenMessage.Content.ReadAsStringAsync();
            var authToken = JsonConvert.DeserializeObject<Api_Pagadito_Response_TokenPagadito>(responseBodyToken);
            if (authToken is null)
            {
                Mensaje = Mensajes.AuthTokenError;
                return null;
            }

            var listaDetails = new List<Detalle>();
            //var details = new Dictionary<string, object>();
            foreach (var item in itemsResponses)
            {
                var detail = new Detalle
                {
                    Quantity = item.Quantity,
                    Description = item.Name,
                    Price = item.PrecioPublico,
                    UrlProduct = "http://posme.net"
                };
                listaDetails.Add(detail);
            }

            var customParam = new Dictionary<string, string>
            {
                { "param1", "value1" }
            };
            //var listParametros = new List<Dictionary<string, string>>
            //{
            //    customParam
            //};
            var simboloMoneda = transactionMaster.CurrencyId switch
            {
                TypeCurrency.Cordoba => Mensajes.MonedaCordoba,
                TypeCurrency.Dolar => Mensajes.MonedaDolar,
                _ => ""
            };
            var ern = $"{VariablesGlobales.CompanyKey}_{DateTime.Now:yyyyMMddHHmmss}";
            var detallesJson = JsonConvert.SerializeObject(listaDetails);
            var parametrosJson = JsonConvert.SerializeObject(customParam);
            var data = new List<KeyValuePair<string, string>>
            {
                new("operation", "41216f8caf94aaa598db137e36d4673e"),
                new("token", authToken.Value),
                new("format_return", "json"),
                new("ern", ern),
                new("amount", transactionMaster.Amount.ToString("N2")),
                new("currency", simboloMoneda),
                new("details", detallesJson),
                new("custom_params", parametrosJson)
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Constantes.UrlPagaditoToken),
                Content = new FormUrlEncodedContent(data)
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            Mensaje = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Api_Pagadito_Response_Exec>(Mensaje);
        }
        catch (Exception e)
        {
            Mensaje = e.Message;
            return null;
        }
    }

    private string GenerateBasicAuthToken(string username, string password)
    {
        var authToken = $"{username}:{password}";
        var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(authToken));
        return base64Token;
    }
}