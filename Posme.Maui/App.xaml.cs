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
            var dataBase = new DataBase();
            DependencyService.Register<NavigationService>();
            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
            dataBase.Init();
            dataBase.InitDownloadTables();
        }
    }
}