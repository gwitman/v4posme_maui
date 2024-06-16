namespace Posme.Maui.Models;

public record DtoTipoDocumento(int TipoDocumento, string Descripcion)
{
    public override string ToString() => Descripcion;
}