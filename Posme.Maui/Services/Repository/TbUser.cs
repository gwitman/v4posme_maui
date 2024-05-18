using System.Diagnostics;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Posme.Maui.Models;
namespace Posme.Maui.Services.Repository;

public class TbUser
{
    private readonly DataBase _dataBase = new();

    public async Task PosMeInsert(CoreAccountMLoginMobileObjUserResponse objUser)
    {
        await _dataBase.Database.InsertAsync(objUser);
    }

    public async Task PosMeUpdate(CoreAccountMLoginMobileObjUserResponse objUser)
    {
        await _dataBase.Database.UpdateAsync(objUser);
    }
    public async Task PosMeOnRemember()
    {
        var listAsync = await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>().ToListAsync();
        if (listAsync.Count <= 0)
        {
            return;
        }

        foreach (var user in listAsync)
        {
            user.Remember = false;
        }

        await _dataBase.Database.UpdateAllAsync(listAsync);
    }

    public async Task<CoreAccountMLoginMobileObjUserResponse> PosmeFindUserRemember()
    {
        return await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>()
            .Where(user => user.Remember)
            .FirstOrDefaultAsync();
    }

    public async Task<CoreAccountMLoginMobileObjUserResponse> PosMeFindUserByNicknameAndPassword(string nickname, string password)
    {
        return await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>()
            .Where(user => user.Nickname == nickname && user.Password == password)
            .FirstOrDefaultAsync();
    }

    public async Task<int> PosMeRowCount()
    {
        return await _dataBase.Database.Table<CoreAccountMLoginMobileObjUserResponse>().CountAsync();
    }

}
