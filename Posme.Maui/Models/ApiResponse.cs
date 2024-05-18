namespace Posme.Maui.Models;

public class ApiResponseUser
{
    public bool Error { get; set; }
    public string Message { get; set; }
    public ObjUser ObjUser { get; set; }
}