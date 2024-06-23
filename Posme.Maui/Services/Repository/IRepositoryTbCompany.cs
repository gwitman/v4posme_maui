using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbCompany : IRepositoryFacade<TbCompany>
{
    
}

public class RepositoryTbCompany(DataBase dataBase) : RepositoryFacade<TbCompany>(dataBase),IRepositoryTbCompany
{
}