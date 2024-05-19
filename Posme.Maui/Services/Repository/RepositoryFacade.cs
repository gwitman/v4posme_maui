namespace Posme.Maui.Services.Repository;

public abstract class RepositoryFacade<T>(DataBase dataBase) : IRepositoryFacade<T> where T : new()
{
    public async Task PosMeInsertAll(List<T> list)
    {
        await dataBase.Database.InsertAllAsync(list);
    }

    public async Task PosMeInsert(T model)
    {
        await dataBase.Database.InsertAsync(model);
    }

    public async Task PosMeUpdate(T model)
    {
        await dataBase.Database.UpdateAsync(model);
    }

    public async Task<bool> PosMeDeleteAll()
    {
        return await dataBase.Database.DeleteAllAsync<T>()>0;
    }

    public async Task<List<T>> PosMeFindAll()
    {
        return await dataBase.Database.Table<T>().ToListAsync();
    }
}