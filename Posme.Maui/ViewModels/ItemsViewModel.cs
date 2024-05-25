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

        private ICommand _loadMoreCommand;
        public ICommand LoadMoreCommand {
            get => _loadMoreCommand;
            set {
                if (_loadMoreCommand != value) {
                    _loadMoreCommand = value;
                    OnPropertyChanged("LoadMoreCommand");
                }
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand CreateDetailFormViewModelCommand { get; }

        private int _lastLoadedIndex = 0;
        private readonly int _loadBatchSize = 8;
        private int _sourceSize;
        private bool _isLoading;

        public ItemsViewModel()
        {
            Items = new List<AppMobileApiMGetDataDownloadItemsResponse>();
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            ExecuteLoadItemsAsync();
            Task.Run(async () =>
            {
                _sourceSize = await _repositoryItems.PosMeCount();
            });
            Title = "Productos";
            CreateDetailFormViewModelCommand = new Command<CreateDetailFormViewModelEventArgs>(CreateDetailFormViewModel);
            SearchCommand = new Command(OnSearchItems);
            LoadMoreCommand = new Command(LoadMore);
        }

        private void LoadMore(object obj)
        {
            _lastLoadedIndex += _loadBatchSize;
            ExecuteLoadItemsAsync();
        }

        public async void ExecuteLoadItemsAsync()
        {
            try
            {
                IsLoading = true;
                if (Items==null)
                {
                    Items = new List<AppMobileApiMGetDataDownloadItemsResponse>();
                }

                var findItems = await _repositoryItems.PosMeFindAll();
                Items.AddRange(findItems);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
                RaisePropertyChanged();
            }
        }

        private List<AppMobileApiMGetDataDownloadItemsResponse> _items;
        public List<AppMobileApiMGetDataDownloadItemsResponse> Items
        {
            get => _items;
            set
            {
                if (_items != value) {
                    _items = value;
                    OnPropertyChanged("Items");
                }
            }
        }

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

        private async void OnSearchItems(object obj)
        {
            IsLoading = true;
            TextSearch = obj.ToString();
            Items = new List<AppMobileApiMGetDataDownloadItemsResponse>();
            var searchItems = await _repositoryItems.PosMeFilterdByItemNumberAndBarCodeAndName(TextSearch);
            Items.AddRange(searchItems);
            IsLoading = false;
        }
    }
}