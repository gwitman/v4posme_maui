namespace Posme.Maui.Models;

public class DtoInvoice
{
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
    
    public DtoCurrency? Currency { get; set; }
    
    public DtoTipoDocumento? TipoDocumento { get; set; }
}