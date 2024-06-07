namespace Posme.Maui.Services.Repository;

public interface IRepositoryFacade<T>
{
    Task PosMeInsertAll(List<T> list);
    Task PosMeInsert(T model);

    Task PosMeUpdate(T model);

    Task PosMeUpdateAll(List<T> list);
    
    Task<bool> PosMeDelete(T model);
    
    Task<bool> PosMeDeleteAll();
    
    Task<List<T>> PosMeFindAll();

    Task<List<T>> PosMeFindStartAndTake(int start, int take);

    Task<int> PosMeCount();
}