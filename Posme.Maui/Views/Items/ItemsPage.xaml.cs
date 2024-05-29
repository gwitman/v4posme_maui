using DevExpress.Maui.Scheduler;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views.Items
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

        public ItemsPage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((ItemsViewModel)BindingContext).OnAppearing(Navigation);
            ((ItemsViewModel)BindingContext).LoadMoreItems();
        }
    }
}