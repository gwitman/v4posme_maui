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

        public ItemsViewModel()
        {
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            Title = "Productos";
            Items = new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>();
            CreateDetailFormViewModelCommand = new Command<CreateDetailFormViewModelEventArgs>(CreateDetailFormViewModel);
            SearchCommand = new Command(OnSearchItems);
            OnBarCode = new Command(OnSearchBarCode);
        }


        public ICommand OnBarCode { get; }
        public ICommand SearchCommand { get; }
        public ICommand CreateDetailFormViewModelCommand { get; }
        public ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse> Items { get; set; }


        AppMobileApiMGetDataDownloadItemsResponse _selectedItem;

        public AppMobileApiMGetDataDownloadItemsResponse SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref this._selectedItem, value, () => RaisePropertyChanged(nameof(SelectedItem)));
        }

        private async void OnSearchBarCode(object obj)
        {
            var barCodePage = new BarCodePage();
            await Navigation!.PushModalAsync(barCodePage);
            if (string.IsNullOrWhiteSpace(VariablesGlobales.BarCode)) return;
            Search = VariablesGlobales.BarCode;
            VariablesGlobales.BarCode = "";
            OnSearchItems(Search);
        }

        private async void OnSearchItems(object? obj)
        {
            IsBusy = true;
            if (obj is not null)
            {
                Search = obj.ToString()!;
            }

            await Task.Run(async () =>
            {
                Items.Clear();
                var searchItems = await _repositoryItems.PosMeFilterdByItemNumberAndBarCodeAndName(Search);
                foreach (var itemsResponse in searchItems)
                {
                    Items.Add(itemsResponse);
                }
            });
            IsBusy = false;
        }

        public async void LoadMoreItems()
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                Items.Clear();
                var newItems = await _repositoryItems.PosMeFindAll();
                foreach (var item in newItems)
                {
                    Items.Add(item);
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

        public void OnAppearing(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}