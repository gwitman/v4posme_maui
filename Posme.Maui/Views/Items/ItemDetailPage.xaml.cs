using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Api;
using Unity;

namespace Posme.Maui.Views.Items
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        DetailFormViewModel ViewModel => ((DetailFormViewModel)BindingContext);
        private readonly IRepositoryItems _repositoryItems;
        private bool _isDeleting;
        private Api_AppMobileApi_GetDataDownloadItemsResponse SelectedItem => (Api_AppMobileApi_GetDataDownloadItemsResponse)ViewModel.Item;

        public ItemDetailPage()
        {
            Title = "Datos de Producto";
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            InitializeComponent();
        }

        private void DeleteItemClick(object? sender, EventArgs e)
        {
            Popup.IsOpen = true;
        }

        private async void DeleteConfirmedClick(object? sender, EventArgs e)
        {
            if (_isDeleting)
                return;
            _isDeleting = true;

            try
            {
                var helper = VariablesGlobales.UnityContainer.Resolve<HelperCore>();
                _isDeleting = await _repositoryItems.PosMeDelete(SelectedItem);
                if (_isDeleting)
                {
                    await helper.PlusCounter();
                }

                ViewModel.Close();
            }
            catch (Exception ex)
            {
                _isDeleting = false;
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void CancelDeleteClick(object? sender, EventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}