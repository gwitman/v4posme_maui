using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views;

public partial class DownloadPage : ContentPage
{
    private readonly RestApiAppMobileApi _restApiDownload;
    public DownloadPage()
    {
        _restApiDownload = new RestApiAppMobileApi();
        InitializeComponent();
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }

    protected override void OnAppearing()
    {
        ((PosMeDownloadViewModel)BindingContext).OnAppearing(Navigation);
    }
}