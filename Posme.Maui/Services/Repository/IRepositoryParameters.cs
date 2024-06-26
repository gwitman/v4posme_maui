﻿using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryParameters : IRepositoryFacade<Api_AppMobileApi_GetDataDownloadParametersResponse>
{
    Task<Api_AppMobileApi_GetDataDownloadParametersResponse?> PosMeFindByKey(string key);
}