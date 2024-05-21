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
        BindingContext = new DownloadViewModel();
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }

    private async void DXButtonBase_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoadingPage());

        if (VariablesGlobales.CantidadTransacciones != 0)
        {
            ((DownloadViewModel)BindingContext).Mensaje = Mensajes.MensajeDownloadCantidadTransacciones;
            ((DownloadViewModel)BindingContext).PopupBackgroundColor = Colors.Red;
            Popup.Show();
            await Navigation.PopModalAsync();
            return;
        }

        var result = await _restApiDownload.GetDataDownload();
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