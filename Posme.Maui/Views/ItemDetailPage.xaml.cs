using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        private DetailFormViewModel ViewModel => ((DetailFormViewModel)BindingContext);
        public AppMobileApiMGetDataDownloadItemsResponse Item => (AppMobileApiMGetDataDownloadItemsResponse)ViewModel.Item;
        public ItemDetailPage(IServiceProvider serviceProvider)
        {
            Title = "Datos de Producto";
            InitializeComponent();
            //BindingContext = new ItemDetailViewModel(serviceProvider);
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