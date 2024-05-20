using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(serviceProvider);
        }
    }
}