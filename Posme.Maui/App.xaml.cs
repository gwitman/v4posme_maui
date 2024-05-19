using DevExpress.Maui.Core;
using DevExpress.Maui.Core.Internal;
using Posme.Maui.Services;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace Posme.Maui
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;

        public App(IServiceProvider services)
        {
            _services = services;
            InitializeComponent();
            
            MainPage = new LoginPage(services);
            UserAppTheme = AppTheme.Light; 
        }

        protected override void OnStart()
        {
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<NavigationService>();
            DependencyService.Register<DataBase>();
            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
            Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
            var dataBase = new DataBase();
            dataBase.Init();
            dataBase.InitDownloadTables();
        }
    }
}
