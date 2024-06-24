using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using DevExpress.Maui.Core;
using Posme.Maui.Models;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Unity;
using Posme.Maui.Services.SystemNames;
using Posme.Maui.Services.Api;
namespace Posme.Maui.ViewModels
{
    public class PosMeItemsViewModel : BaseViewModel
    {
        private readonly IRepositoryItems _repositoryItems;

        public PosMeItemsViewModel()
        {
            _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
            Title = "Productos";
            Items = new ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse>();
            CreateDetailFormViewModelCommand = new Command<CreateDetailFormViewModelEventArgs>(CreateDetailFormViewModel);
            SearchCommand = new Command(OnSearchItems);
            OnBarCode = new Command(OnSearchBarCode);
        }


        public ICommand OnBarCode { get; }
        public ICommand SearchCommand { get; }
        public ICommand CreateDetailFormViewModelCommand { get; }
        public ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse> Items { get; set; }


        private Api_AppMobileApi_GetDataDownloadItemsResponse? _selectedItem;

        public Api_AppMobileApi_GetDataDownloadItemsResponse? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private async void OnSearchBarCode(object obj)
        {
            var barCodePage = new BarCodePage();
            await Navigation!.PushModalAsync(barCodePage);
            var bar = await barCodePage.WaitForResultAsync();
            Search = bar!;
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

        private async void LoadMoreItems()
        {
            try
            {
                IsBusy = true;
                Items.Clear();
                var newItems = await _repositoryItems.PosMeFindAll();
                await Task.Run(() =>
                {
                    foreach (var item in newItems)
                    {
                        Items.Add(item);
                    }
                });

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void CreateDetailFormViewModel(CreateDetailFormViewModelEventArgs e)
        {
            if (e.DetailFormType != DetailFormType.Edit) return;
            var eItem = (Api_AppMobileApi_GetDataDownloadItemsResponse)e.Item;
            var editedContact = _repositoryItems.PosMeFindByItemNumber(eItem.ItemNumber);
            e.Result = new DetailEditFormViewModel(editedContact, isNew: false);
        }

        public void OnAppearing(INavigation navigation)
        {
            Navigation = navigation;
            LoadMoreItems();
        }
    }
}