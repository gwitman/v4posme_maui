using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryParameters(DataBase dataBase) : RepositoryFacade<Api_AppMobileApi_GetDataDownloadParametersResponse>(dataBase), IRepositoryParameters
{
}