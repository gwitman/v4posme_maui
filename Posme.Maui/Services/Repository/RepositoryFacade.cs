﻿namespace Posme.Maui.Services.Repository;

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

    public async Task PosMeUpdateAll(List<T> list)
    {
        await dataBase.Database.UpdateAllAsync(list);
    }

    public async Task<bool> PosMeDelete(T model)
    {
        return await dataBase.Database.DeleteAsync(model)>0;
    }
    public async Task<bool> PosMeDeleteAll()
    {
        return await dataBase.Database.DeleteAllAsync<T>()>0;
    }

    public async Task<List<T>> PosMeFindAll()
    {
        return await dataBase.Database.Table<T>().ToListAsync();
    }

    public async Task<List<T>> PosMeFindStartAndTake(int lastLoadedIndex, int loadBatchSize)
    {
        return await dataBase.Database.Table<T>().Skip(lastLoadedIndex).Take(loadBatchSize).ToListAsync();
    }

    public async Task<int> PosMeCount()
    {
        return await dataBase.Database.Table<T>().CountAsync();
    }
}