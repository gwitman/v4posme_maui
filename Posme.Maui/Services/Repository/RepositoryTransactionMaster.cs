using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryTbTransactionMaster(DataBase dataBase) : RepositoryFacade<TbTransactionMaster>(dataBase),IRepositoryTbTransactionMaster
{
}