using Posme.Maui.Services.Repository;
using System.Runtime.Intrinsics.Arm;
using Posme.Maui.Models;

namespace Posme.Maui.Services.Helpers;

public class Helper(IRepositoryTbParameterSystem repositoryParameters)
{
    public async Task<int> GetCounter()
    {
        var find = await repositoryParameters.PosMeFindCounter();
        return Convert.ToInt32(find.Value);
    }

    public async Task PlusCounter()
    {
        var find = await repositoryParameters.PosMeFindCounter();
        var value = Convert.ToInt32(find.Value) + 1;
        find.Value = $"{value}";
        await repositoryParameters.PosMeUpdate(find);
    }

    public async Task<string> ParseUrl(string url)
    {
        var find = await repositoryParameters.PosMeFindAccessPoint();
        var urlTaret = url.Replace(Constantes.UrlBase, find.Value);
        return urlTaret;
    }

    public async Task<string> GetCodigoAbono()
    {

        var find = await repositoryParameters.PosMeFindCodigoAbono();
        var codigo = find.Value!;

        if (codigo.IndexOf("-") < 0 )
        throw new Exception(Mensajes.MnesajeCountadoDeAbonoMalFormado);

        

        var prefix  = find.Value!.Split("-")[0];
        var counter = find.Value!.Split("-")[1];
        var numero  = Convert.ToInt32(counter);
        numero      += 1;
        var nuevoCodigoAbono = prefix + "-" + Convert.ToString(numero).PadLeft(8, '0');
        find.Value  = nuevoCodigoAbono;
        await repositoryParameters.PosMeUpdate(find);

        return codigo;
    }
}