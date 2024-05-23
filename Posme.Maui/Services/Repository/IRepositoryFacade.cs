namespace Posme.Maui.Services.Repository;

public interface IRepositoryFacade<T>
{
    Task PosMeInsertAll(List<T> list);
    Task PosMeInsert(T model);

    Task PosMeUpdate(T model);

    Task<bool> PosMeDelete(T model);
    
    Task<bool> PosMeDeleteAll();
    
    Task<List<T>> PosMeFindAll();

    Task<int> PosMeCount();
}