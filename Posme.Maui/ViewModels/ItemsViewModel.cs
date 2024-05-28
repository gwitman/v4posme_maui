using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IRepositoryItems _repositoryItems;
        private INavigation? _navigation;
        public ItemsViewModel()
        {
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            Title = "Productos";
            Items=new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>();
            CreateDetailFormViewModelCommand = new Command<CreateDetailFormViewModelEventArgs>(CreateDetailFormViewModel);
            SearchCommand = new Command(OnSearchItems);
            OnBarCode = new Command(OnSearchBarCode);
        }

        private async void OnSearchBarCode(object obj)
        {
            var barCodePage = new BarCodePage();
            await _navigation!.PushModalAsync(barCodePage);
            if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
            TextSearch = VariablesGlobales.BarCode;
            VariablesGlobales.BarCode = "";
            OnSearchItems(TextSearch);
        }

        private async void OnSearchItems(object? obj)
        {
            IsBusy = true;
            if (obj is not null)
            {
                TextSearch = obj.ToString(); 
            }
            Items.Clear();
            var searchItems =
                await _repositoryItems.PosMeFilterdByItemNumberAndBarCodeAndName(TextSearch);
            await Task.Run(() =>
            {
                foreach (var itemsResponse in searchItems)
                {
                    Items.Add(itemsResponse);
                }
            });
            IsBusy = false;
        }
        private async void LoadMoreItems()
        {
            IsBusy = true;
            Items.Clear();
            var newItems = await _repositoryItems.PosMeFindAll();
            await Task.Run(() =>
            {
                foreach (var itemsResponse in newItems)
                {
                    Items.Add(itemsResponse);
                }
            });
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

        public ICommand OnBarCode { get; }
        public ICommand SearchCommand { get; }
        public ICommand CreateDetailFormViewModelCommand { get; }
        public ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse> Items { get; set; }

        private string? _textSearch;

        public string? TextSearch
        {
            get => _textSearch;
            set
            {
                SetProperty(ref _textSearch, value);
                RaisePropertyChanged();
            }
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

        public void OnAppearing(INavigation navigation)
        {
            _navigation = navigation;
            LoadMoreItems();
        }
    }
}