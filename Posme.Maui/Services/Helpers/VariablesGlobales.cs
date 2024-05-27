using Posme.Maui.Models;
using Unity;

namespace Posme.Maui.Services.Helpers;

public static class VariablesGlobales
{
    public static string? CompanyKey;
    public static CoreAccountMLoginMobileObjUserResponse? User;
    public static int CantidadTransacciones = 0;
    public static readonly UnityContainer UnityContainer;
    public static string? BarCode;

    static VariablesGlobales()
    {
        UnityContainer = new UnityContainer();
    }
}