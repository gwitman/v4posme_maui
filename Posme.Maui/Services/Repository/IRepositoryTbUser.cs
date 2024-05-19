using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbUser : IRepositoryFacade<CoreAccountMLoginMobileObjUserResponse>
{

    Task PosMeOnRemember();

    Task<CoreAccountMLoginMobileObjUserResponse?> PosmeFindUserRemember();

    Task<CoreAccountMLoginMobileObjUserResponse?> PosMeFindUserByNicknameAndPassword(string nickname,
        string password);

    Task<int> PosMeRowCount();
}