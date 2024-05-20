using Posme.Maui.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Posme.Maui.Services.Repository;

namespace Posme.Maui.ViewModels
{
    public class SchedulerViewModel : BaseViewModel
    {
        private IRepositoryItems _repositoryItems;
        public SchedulerViewModel(IServiceProvider serviceProvider)
        {
            Title = "Scheduler";
            Items = new ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse>();
            _repositoryItems = serviceProvider.GetService<IRepositoryItems>();
        }

        public ObservableCollection<AppMobileApiMGetDataDownloadItemsResponse> Items { get; private set; }

        async public void OnAppearing()
        {
            IEnumerable<AppMobileApiMGetDataDownloadItemsResponse> items = await _repositoryItems.PosMeFindAll();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}