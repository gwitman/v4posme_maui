﻿using Posme.Maui.Models;

namespace Posme.Maui.Services.Repository;

public interface IRepositoryTbCustomer : IRepositoryFacade<AppMobileApiMGetDataDownloadCustomerResponse>
{
    Task<AppMobileApiMGetDataDownloadCustomerResponse> PosMeFindCustomer(string customerNumber);

    Task<List<AppMobileApiMGetDataDownloadCustomerResponse>> PosMeFilterBySearch(string search);

    Task<List<AppMobileApiMGetDataDownloadCustomerResponse>> PosMeFilterByInvoice();
}