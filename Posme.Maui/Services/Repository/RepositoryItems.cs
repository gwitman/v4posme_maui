using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public class RepositoryItems(DataBase dataBase) : RepositoryFacade<AppMobileApiMGetDataDownloadItemsResponse>(dataBase),IRepositoryItems
{
}