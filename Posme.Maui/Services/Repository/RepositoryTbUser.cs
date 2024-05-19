using Posme.Maui.Models;


namespace Posme.Maui.Services.Repository;

public class RepositoryTbUser(DataBase dataBase) : RepositoryFacade<CoreAccountMLoginMobileObjUserResponse>(dataBase),IRepositoryTbUser
{

    public async Task PosMeOnRemember()
    {
        var listAsync = await dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>().ToListAsync();
        if (listAsync.Count <= 0)
        {
            return;
        }

        foreach (var user in listAsync)
        {
            user.Remember = false;
        }

        await dataBase.Database.UpdateAllAsync(listAsync);
    }

    public async Task<CoreAccountMLoginMobileObjUserResponse?> PosmeFindUserRemember()
    {
        return await dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>()
            .Where(user => user.Remember)
            .FirstOrDefaultAsync();
    }

    public async Task<CoreAccountMLoginMobileObjUserResponse?> PosMeFindUserByNicknameAndPassword(string nickname,
        string password)
    {
        return await dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>()
            .Where(user => user.Nickname == nickname && user.Password == password)
            .FirstOrDefaultAsync();
    }

    public async Task<int> PosMeRowCount()
    {
        return await dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>().CountAsync();
    }
}