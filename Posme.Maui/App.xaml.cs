using Posme.Maui.Services;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;
using Posme.Maui.Views.Items;
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
            var dataBase = new DataBase();
            dataBase.Init();
            dataBase.InitDownloadTables();
            MainPage = new LoginPage();
            UserAppTheme = AppTheme.Light;
        }

        protected override void OnStart()
        {
            DependencyService.Register<NavigationService>();
            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
        }
    }
}