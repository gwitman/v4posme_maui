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
    private readonly RestApiDownload _restApiDownload;

    private readonly IServiceProvider _services;
    
    public DownloadPage(IServiceProvider services)
    {
        _services = services; 
        _restApiDownload = new RestApiDownload(services);
        InitializeComponent();
        BindingContext = new DownloadViewModel();
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }

    private async void DXButtonBase_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoadingPage());
        var result = await _restApiDownload.DownloadData();
        if (result)
        {
            ((DownloadViewModel)BindingContext).PopupBackgroundColor = Colors.Green;
            ((DownloadViewModel)BindingContext).Mensaje = Mensajes.MensajeDownloadSuccess;
        }
        else
        {
            ((DownloadViewModel)BindingContext).Mensaje = Mensajes.MensajeDownloadError;
            ((DownloadViewModel)BindingContext).PopupBackgroundColor = Colors.Red;
        }

        Popup.Show();
        await Navigation.PopModalAsync();
    }
}