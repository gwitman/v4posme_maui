using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Unity;
using Debug = System.Diagnostics.Debug;

namespace Posme.Maui.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IRepositoryItems _repositoryItems;
        private int _sourceSize;
        private int _lastLoadedIndex;
        private const int LoadBatchSize = 50;

        public ItemsViewModel()
        {
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            Title = "Productos";
            Items = new List<AppMobileApiMGetDataDownloadItemsResponse>();
            Task.Run(async () =>
            {
                _sourceSize = await _repositoryItems.PosMeCount();
                LoadMore();
            });
            CreateDetailFormViewModelCommand = new Command<CreateDetailFormViewModelEventArgs>(CreateDetailFormViewModel);
            SearchCommand = new Command(OnSearchItems);
            LoadMoreCommand = new Command(LoadMore, CanLoadMore);
        }
        
        private async void OnSearchItems(object obj)
        {
            IsBusy = true;
            TextSearch = obj.ToString();
            Items = new List<AppMobileApiMGetDataDownloadItemsResponse>();
            var searchItems =
                await _repositoryItems.PosMeFilterdByItemNumberAndBarCodeAndName(TextSearch);
            Items.AddRange(searchItems);
            IsBusy = false;
        }
        private bool CanLoadMore()
        {
            return Items.Count < _sourceSize;
        }

        private void LoadMore()
        {
            LoadMoreItems();
            _lastLoadedIndex += LoadBatchSize;
        }

        private async void LoadMoreItems()
        {
            IsBusy = true;
            Items.Clear();
            var newItems = await _repositoryItems.PosMeFindStartAndTake(_lastLoadedIndex, LoadBatchSize);
            Items.AddRange(newItems);
            IsBusy = false;
        }
        private void CreateDetailFormViewModel(CreateDetailFormViewModelEventArgs e)
        {
            if (e.DetailFormType == DetailFormType.Edit)
            {
                var eItem = (AppMobileApiMGetDataDownloadItemsResponse)e.Item;
                var editedContact = _repositoryItems.PosMeFindByItemNumber(eItem.ItemNumber);
                e.Result = new DetailEditFormViewModel(editedContact, isNew: false);
            }
        }

        public ICommand SearchCommand { get; }

        public ICommand CreateDetailFormViewModelCommand { get; }

        public ICommand LoadMoreCommand { get; set; }

        public List<AppMobileApiMGetDataDownloadItemsResponse> Items { get; set; }

        private string? _textSearch;

        public string? TextSearch
        {
            get => _textSearch;
            set => SetProperty(ref _textSearch, value);
        }

        AppMobileApiMGetDataDownloadItemsResponse? _selectedItem;

        public AppMobileApiMGetDataDownloadItemsResponse? SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref this._selectedItem, value);
                RaisePropertyChanged();
            }
        }
    }
}