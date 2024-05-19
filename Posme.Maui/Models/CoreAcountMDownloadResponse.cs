namespace Posme.Maui.Models;

public class CoreAcountMDownloadResponse
{
    public bool Error { get; set; }
    public string? Message { get; set; }
    public List<CoreAcountItems> ListItem { get; set; } = [];
    public List<CoreAcountCustomers> ListCustomer { get; set; } = [];
    public List<CoreAcountParameters> ListParameter { get; set; } = [];
    public List<CoreAcountDocumentCredit> ListDocumentCredit { get; set; } = [];
    public List<CoreAcountDocumentCreditAmortization> ListDocumentCreditAmortization { get; set; } = [];
}