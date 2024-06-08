using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryTransactionMaster(DataBase dataBase) : RepositoryFacade<TbTransactionMaster>(dataBase),IRepositoryTransactionMaster
{
}