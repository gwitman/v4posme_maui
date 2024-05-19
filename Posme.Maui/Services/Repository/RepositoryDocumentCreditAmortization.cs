using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryDocumentCreditAmortization(DataBase dataBase) : RepositoryFacade<CoreAcountDocumentCreditAmortization>(dataBase),IRepositoryDocumentCreditAmortization
{
}