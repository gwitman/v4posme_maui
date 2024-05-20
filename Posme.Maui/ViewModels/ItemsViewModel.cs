using System.Collections.ObjectModel;
using Posme.Maui.Models;
using Posme.Maui.Services.Repository;
using Debug = System.Diagnostics.Debug;

namespace Posme.Maui.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        AppMobileApiMGetDataDownloadItemsResponse _selectedItem;
        private string? _textSearch;
        private IRepositoryItems? _repositoryItems;

        public ItemsViewModel(IServiceProvider services)
        {
            Title = "Productos";
            SearchCommand = new Command(OnSearchItems);
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<AppMobileApiMGetDataDownloadItemsResponse>(OnItemSelected);
            _repositoryItems = services.GetService<IRepositoryItems>();
            Items = new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>();
        }

        public ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse> Items
        {
            get;
        }

        public Command LoadItemsCommand { get; }
        public Command SearchCommand { get; }

        public Command AddItemCommand { get; }

        public Command<AppMobileApiMGetDataDownloadItemsResponse> ItemTapped { get; }

        public string? TextSearch
        {
            get => _textSearch;
            set => SetProperty(ref _textSearch, value);
        }

        public AppMobileApiMGetDataDownloadItemsResponse SelectedItem
        {
            get => this._selectedItem;
            set
            {
                SetProperty(ref this._selectedItem, value);
                OnItemSelected(value);
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
            LoadItemsCommand.Execute(null);
        }

        private async void OnSearchItems(object obj)
        {
            TextSearch = obj.ToString();
            Items.Clear();
            var searchItems = await _repositoryItems!.PosMeFilterdByItemNumber(TextSearch);
            foreach (var item in searchItems)
            {
                Items.Add(item);
            }
        }


        private async void ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var items = await _repositoryItems!.PosMeFindAll();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void OnItemSelected(AppMobileApiMGetDataDownloadItemsResponse? item)
        {
            if (item == null)
                return;
            await Navigation.NavigateToAsync<ItemDetailViewModel>(item.ItemNumber!);
        }
    }
}