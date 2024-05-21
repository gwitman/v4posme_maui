using CommunityToolkit.Maui;
using DevExpress.Maui;
using DevExpress.Maui.Core;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Posme.Maui.Services;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.Repository;
using Posme.Maui.ViewModels;
using Posme.Maui.Views;
using Unity;

namespace Posme.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ThemeManager.ApplyThemeToSystemBars = false;
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseDevExpress(useLocalization: true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("roboto-regular.ttf", "Roboto");
                    fonts.AddFont("roboto-medium.ttf", "Roboto-Medium");
                    fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                    fonts.AddFont("univia-pro-regular.ttf", "Univia-Pro");
                    fonts.AddFont("univia-pro-medium.ttf", "Univia-Pro Medium");
                });
            
            VariablesGlobales.UnityContainer.RegisterType<IRepositoryTbUser, RepositoryTbUser>();
            VariablesGlobales.UnityContainer.RegisterType<IRepositoryTbCustomer, RepositoryTbCustomer>();
            VariablesGlobales.UnityContainer.RegisterType<IRepositoryDocumentCreditAmortization, RepositoryDocumentCreditAmortization>();
            VariablesGlobales.UnityContainer.RegisterType<IRepositoryDocumentCredit, RepositoryDocumentCredit>();
            VariablesGlobales.UnityContainer.RegisterType<IRepositoryItems, RepositoryItems>();
            VariablesGlobales.UnityContainer.RegisterType<IRepositoryParameters, RepositoryParameters>();
            VariablesGlobales.UnityContainer.RegisterSingleton<DataBase>();
            VariablesGlobales.UnityContainer.RegisterSingleton<DownloadPage>();
            VariablesGlobales.UnityContainer.RegisterSingleton<SchedulerPage>();
            VariablesGlobales.UnityContainer.RegisterSingleton<ItemDetailPage>();
            VariablesGlobales.UnityContainer.RegisterSingleton<ItemsPage>();
            VariablesGlobales.UnityContainer.RegisterSingleton<ItemsViewModel>();
            VariablesGlobales.UnityContainer.RegisterSingleton<ItemDetailViewModel>();
            DevExpress.Maui.Charts.Initializer.Init();
            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();
            DevExpress.Maui.DataGrid.Initializer.Init();
            DevExpress.Maui.Scheduler.Initializer.Init();
            return builder.Build();
        }
    }
}