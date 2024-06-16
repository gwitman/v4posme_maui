using System.Collections.ObjectModel;

namespace Posme.Maui.Models;

public class ViewTempDtoInvoice
{
    public ViewTempDtoInvoice()
    {
        Items = new();
    }

    public Api_AppMobileApi_GetDataDownloadCustomerResponse? CustomerResponse { get; set; }

    public string? CustomerNumber { get; set; }

    public string? Identification { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? NombreCompleto
    {
        get => $"{FirstName} {LastName}";
    }

    public decimal Balance { get; set; }

    public string? Comentarios { get; set; } = string.Empty;

    public string? Referencia { get; set; } = string.Empty;

    public DtoCatalogItem? Currency { get; set; }

    public DtoCatalogItem? TipoDocumento { get; set; }

    public ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse> Items { get; }

    public void ClearItems()
    {
        Items.Clear();
    }
}