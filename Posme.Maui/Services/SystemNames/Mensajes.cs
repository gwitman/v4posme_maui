﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Posme.Maui.Services.SystemNames;

public static class Mensajes
{
    public const string MensajeCredencialesInvalida = "Credenciales incorrectas o nombre de compañía no existe. Inténtalo nuevamente.";
    public const string MensajeSinDatosTabla = "No hay datos ingresados en el celular para buscar usuario, conectarse a internet para descargar datos";
    public const string MensajeDownloadError = "No fue posible descargar los datos, revise su conexion a internet e intente nuevamente.";
    public const string MensajeDownloadSuccess = "Se han descargado los datos de forma correcta.";
    public const string MensajeDownloadCantidadTransacciones = "No puede realizar la descarga sin antes subir la información pendiente.";
    public const string MensajeParametrosGuardar = "Se han guardado los parametros de forma correcta";
    public const string MensajeDocumentCreditCustomerVacio = "No hay datos de facturación con el cliente seleccionado.";
    public const string MensajeDocumentCreditAmortizationVacio = "No hay datos de detalle para abono de factura con el documento seleccionado";
    public const string MnesajeCountadoDeAbonoMalFormado = "El countador de los abonos tiene un formato incorrecto, ABC-#";
    public const string AnularAbonoValidacion = "No puede eliminar este abono, intente nuevamente";
    public const string MensajeMontoMayorSaldoFinal = "No se puede ingresar un monto mayor al saldo final";
    public const string MensajeSaldoNegativo = "No se puede ingresar un saldo negativo";
    public const string MensajeValorZero = "No puede ingresar un valor en 0";
}
