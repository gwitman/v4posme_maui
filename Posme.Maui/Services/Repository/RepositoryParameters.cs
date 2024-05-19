using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryParameters(DataBase dataBase) : RepositoryFacade<CoreAcountParameters>(dataBase), IRepositoryParameters
{
}