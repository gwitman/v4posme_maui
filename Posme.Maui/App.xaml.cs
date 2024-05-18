using DevExpress.Maui.Core;
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
            
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<NavigationService>();
            DependencyService.Register<DataBase>();
            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
            Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
            new DataBase().Init();
        }
    }
}
