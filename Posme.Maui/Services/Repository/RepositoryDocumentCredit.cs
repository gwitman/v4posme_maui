using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryDocumentCredit(DataBase dataBase) : RepositoryFacade<CoreAcountDocumentCredit>(dataBase),IRepositoryDocumentCredit
{
}