using System.Diagnostics;
using System.Web;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IRepositoryItems _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
        
        private AppMobileApiMGetDataDownloadItemsResponse _selectedItem;

        public AppMobileApiMGetDataDownloadItemsResponse SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged();
            }
        }

        private async Task LoadItemId(string? itemId)
        {
            try
            {
                _selectedItem = await _repositoryItems.PosMeFindByItemNumber(itemId);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        public override async Task InitializeAsync(object parameter)
        {
            await LoadItemId(parameter as string);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = HttpUtility.UrlDecode(query["id"] as string);
            await LoadItemId(id);
        }
    }
}