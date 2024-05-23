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
    private readonly DownloadViewModel _viewModel;
    public DownloadPage()
    {
        _restApiDownload = new RestApiAppMobileApi();
        InitializeComponent();
        BindingContext =_viewModel= new DownloadViewModel();
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
            _viewModel.Mensaje = Mensajes.MensajeDownloadCantidadTransacciones;
            _viewModel.PopupBackgroundColor = Colors.Red;
            Popup.Show();
            await Navigation.PopModalAsync();
            return;
        }

        var result = await _restApiDownload.GetDataDownload();
        if (result)
        {
            _viewModel.PopupBackgroundColor = Colors.Green;
            _viewModel.Mensaje = Mensajes.MensajeDownloadSuccess;
        }
        else
        {
            _viewModel.Mensaje = Mensajes.MensajeDownloadError;
            _viewModel.PopupBackgroundColor = Colors.Red;
        }

        Popup.Show();
        await Navigation.PopModalAsync();
    }
}