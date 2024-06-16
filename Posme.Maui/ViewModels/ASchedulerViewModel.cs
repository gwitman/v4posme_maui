using Posme.Maui.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Posme.Maui.Services.Repository;

namespace Posme.Maui.ViewModels
{
    public class ASchedulerViewModel : BaseViewModel
    {
        private IRepositoryItems _repositoryItems;
        public ASchedulerViewModel(IServiceProvider serviceProvider)
        {
            Title = "Scheduler";
            Items = new ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse>();
            _repositoryItems = serviceProvider.GetService<IRepositoryItems>();
        }

        public ObservableCollection<Api_AppMobileApi_GetDataDownloadItemsResponse> Items { get; private set; }

        async public void OnAppearing()
        {
            IEnumerable<Api_AppMobileApi_GetDataDownloadItemsResponse> items = await _repositoryItems.PosMeFindAll();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}