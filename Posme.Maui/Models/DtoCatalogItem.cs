namespace Posme.Maui.Models;

public record DtoCatalogItem(int Key, string Name, string Simbolo)
{
    public override string ToString() => Name;
}