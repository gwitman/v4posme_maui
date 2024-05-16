using Posme.Maui.Services;
using Posme.Maui.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace Posme.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<NavigationService>();
            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
            Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
            var dataBase = new DataBase();
            _ = dataBase.Init();
            MainPage = new LoginPage();
        }
    }
}
