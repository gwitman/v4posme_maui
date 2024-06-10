﻿using Posme.Maui.Services;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Views;
using Posme.Maui.Views.Abonos;
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
            DependencyService.Register<IPrintService>();
            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
            Routing.RegisterRoute(typeof(CustomerDetailInvoicePage).FullName, typeof(CustomerDetailInvoicePage));
            Routing.RegisterRoute(typeof(AbonosPage).FullName, typeof(AbonosPage));
            Routing.RegisterRoute(typeof(CreditDetailInvoicePage).FullName, typeof(CreditDetailInvoicePage));
            Routing.RegisterRoute(typeof(AplicarAbonoPage).FullName, typeof(AplicarAbonoPage));
            Routing.RegisterRoute(typeof(ValidarAbonoPage).FullName, typeof(ValidarAbonoPage));
            VariablesGlobales.BarCode = string.Empty;
        }
    }
}