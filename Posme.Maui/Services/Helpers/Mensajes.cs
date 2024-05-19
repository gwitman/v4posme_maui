using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Posme.Maui.Services.Helpers;

public static class Mensajes
{
    public const string MensajeCredencialesInvalida = "Credenciales incorrectas o nombre de compañía no existe. Inténtalo nuevamente.";
    public const string MensajeSinDatosTabla = "No hay datos ingresados en el celular para buscar usuario, conectarse a internet para descargar datos";
    public const string MensajeDownloadError = "No fue posible descargar los datos, revise su conexion a internet e intente nuevamente.";
    public const string MensajeDownloadSuccess = "Se han descargado los datos de forma correcta.";
}
