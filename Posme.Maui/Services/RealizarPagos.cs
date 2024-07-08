using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Posme.Maui.Models;
using Posme.Maui.Services.SystemNames;

namespace Posme.Maui.Services;

public class RealizarPagos
{
    public string Mensaje { get; set; } = string.Empty;

    public async Task<bool> GenerarUrl(List<Api_AppMobileApi_GetDataDownloadItemsResponse> itemsResponses, TbTransactionMaster transactionMaster)
    {
        try
        {
            var client = new HttpClient();
            var details = new Dictionary<string, object>();
            var username = "test";
            var password = "password";
            foreach (var item in itemsResponses)
            {
                details.Add("quantity", item.Quantity);
                details.Add("description", item.Name);
                details.Add("price", item.PrecioPublico);
            }

            var simboloMoneda = "";
            if (transactionMaster.CurrencyId == TypeCurrency.Cordoba)
            {
                simboloMoneda = "NIO";
            }

            if (transactionMaster.CurrencyId == TypeCurrency.Dolar)
            {
                simboloMoneda = "USD";
            }

            var data = new Dictionary<string, object>
            {
                { "ern", transactionMaster.TransactionNumber! },
                { "amount", transactionMaster.Amount },
                { "currency", simboloMoneda },
                { "extended_expiration", false },
                { "details", details }
            };
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            var response = await client.PostAsync(Constantes.UrlPagadito, content);
            response.EnsureSuccessStatusCode();
            Mensaje = await response.Content.ReadAsStringAsync();
            return true;
        }
        catch (Exception e)
        {
            Mensaje = e.Message;
            return false;
        }
    }
}