using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Views.Items
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        DetailFormViewModel ViewModel => ((DetailFormViewModel)BindingContext);
        private readonly IRepositoryItems _repositoryItems;
        private bool _isDeleting;
        private AppMobileApiMGetDataDownloadItemsResponse SelectedItem => (AppMobileApiMGetDataDownloadItemsResponse)ViewModel.Item;

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
                _isDeleting = await _repositoryItems.PosMeDelete(SelectedItem);
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