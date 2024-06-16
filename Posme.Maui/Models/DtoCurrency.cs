namespace Posme.Maui.Models;

public record DtoCurrency(int CurrencyId, string CurrencyName)
{
    public override string ToString() => CurrencyName;
}