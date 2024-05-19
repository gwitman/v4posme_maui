using DevExpress.Maui;
using DevExpress.Maui.Core;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Posme.Maui.Services;
using Posme.Maui.Services.Repository;
using Posme.Maui.Views;

namespace Posme.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //ThemeManager.ApplyThemeToSystemBars = true;

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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
            builder.Services.AddTransient<IRepositoryTbUser, RepositoryTbUser>();
            builder.Services.AddTransient<IRepositoryTbCustomer, RepositoryTbCustomer>();
            builder.Services.AddTransient<IRepositoryDocumentCreditAmortization, RepositoryDocumentCreditAmortization>();
            builder.Services.AddTransient<IRepositoryDocumentCredit, RepositoryDocumentCredit>();
            builder.Services.AddTransient<IRepositoryItems, RepositoryItems>();
            builder.Services.AddTransient<IRepositoryParameters, RepositoryParameters>();
            builder.Services.AddSingleton<DataBase>();
            builder.Services.AddSingleton<DownloadPage>();
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