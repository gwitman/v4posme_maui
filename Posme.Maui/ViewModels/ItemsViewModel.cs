using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;
using Debug = System.Diagnostics.Debug;

namespace Posme.Maui.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        
        private readonly IRepositoryItems _repositoryItems;
        
        private INavigation _navigationPage;
        public ICommand LoadItemsCommand { get; }
        public ICommand SearchCommand { get; }

        public ICommand AddItemCommand { get; }
        public ICommand ValidateAndSaveCommand { get; }
        public ICommand CreateDetailFormViewModelCommand { get; }
        
        public ItemsViewModel()
        {
            Title = "Productos";
            CreateDetailFormViewModelCommand = new Command<CreateDetailFormViewModelEventArgs>(CreateDetailFormViewModel);
            ValidateAndSaveCommand = new Command<ValidateItemEventArgs>(ValidateAndSave);
            SearchCommand = new Command(OnSearchItems);
            LoadItemsCommand = new Command(ExecuteLoadItemsAsync);
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            Items = new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>();
        }

        private async void ExecuteLoadItemsAsync()
        {
            await ExecuteLoadItemsCommand();
        }

        async void ValidateAndSave(ValidateItemEventArgs e)
        {
        }

        private void CreateDetailFormViewModel(CreateDetailFormViewModelEventArgs e)
        {
            if (e.DetailFormType == DetailFormType.Edit) {
                var eItem = (AppMobileApiMGetDataDownloadItemsResponse)e.Item;
                var editedContact = _repositoryItems.PosMeFindByItemNumber(eItem.ItemNumber);
                e.Result = new DetailEditFormViewModel(editedContact, isNew: false, null);
            }
        }

        private ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse> _items;
        public ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse> Items
        {
            get => _items;
            set
            {
                SetProperty(ref _items, value);
                RaisePropertyChanged();
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

        public async void OnAppearing(INavigation navigation)
        {
            _navigationPage = navigation;
            SelectedItem = null;
            await ExecuteLoadItemsCommand();
        }

        private async void OnSearchItems(object obj)
        {
            IsBusy = true;
            TextSearch = obj.ToString();
            Items.Clear();
            var searchItems = await _repositoryItems!.PosMeFilterdByItemNumber(TextSearch);
            Items = new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>(searchItems);
            IsBusy = false;
        }


        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var items = await _repositoryItems!.PosMeFindAll();
                Items = new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>(items);
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
    }
}