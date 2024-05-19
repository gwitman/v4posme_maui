using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryParameters(DataBase dataBase) : RepositoryFacade<AppMobileApiMGetDataDownloadParametersResponse>(dataBase), IRepositoryParameters
{
}