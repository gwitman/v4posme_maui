using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        DetailFormViewModel ViewModel => ((DetailFormViewModel)BindingContext);
        bool isDeleting;
        public AppMobileApiMGetDataDownloadItemsResponse SelectedItem => (AppMobileApiMGetDataDownloadItemsResponse)ViewModel.Item;

        public ItemDetailPage()
        {
            Title = "Datos de Producto";
            InitializeComponent();
        }

        private void SaveItemClick(object? sender, EventArgs e)
        {
        }

        private void DeleteItemClick(object? sender, EventArgs e)
        {
        }

        private void DeleteConfirmedClick(object? sender, EventArgs e)
        {
        }

        private void CancelDeleteClick(object? sender, EventArgs e)
        {
        }
    }
}