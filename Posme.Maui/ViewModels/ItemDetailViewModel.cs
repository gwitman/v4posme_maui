using System.Diagnostics;
using System.Web;
using Posme.Maui.Services.Repository;

namespace Posme.Maui.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IRepositoryItems? _repositoryItems;
        private string? _barCode;
        private string? _name;
        private string? _itemNumber;
        private decimal? _precioPublico;

        public ItemDetailViewModel(IServiceProvider serviceProvider)
        {
            _repositoryItems = serviceProvider.GetService<IRepositoryItems>();
        }

        public decimal? PrecioPublico
        {
            get => _precioPublico;
            set => SetProperty(ref _precioPublico, value);
        }

        public string ItemNumber
        {
            get => _itemNumber;
            set => SetProperty(ref _itemNumber, value);
        }

        public string BarCode
        {
            get => this._barCode;
            set => SetProperty(ref this._barCode, value);
        }

        public string Name
        {
            get => this._name;
            set => SetProperty(ref this._name, value);
        }

        public async Task LoadItemId(string itemId)
        {
            try
            {
                var item = await _repositoryItems!.PosMeFindByItemNumber(itemId);
                ItemNumber = item.ItemNumber!;
                BarCode = item.BarCode!;
                Name = item.Name!;
                PrecioPublico = item.PrecioPublico;
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