namespace Posme.Maui.Models;

public record DtoAbono(
    string CodigoAbono,
    string CustomerNumber,
    string FirstName,
    string LastName,
    string Identification,
    DateTime Fecha,
    string DocumentNumber,
    string CurrencyName,
    decimal MontoAplicar,
    decimal SaldoInicial,
    decimal SaldoFinal,
    string Description);