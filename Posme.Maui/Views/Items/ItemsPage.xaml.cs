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
            Title = "Productos";
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((PosMeItemsViewModel)BindingContext).OnAppearing(Navigation);
        }
    }
}