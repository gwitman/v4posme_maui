namespace Posme.Maui.Models;

public record DtoCatalogItem(int key, string Name)
{
    public override string ToString() => Name;
}