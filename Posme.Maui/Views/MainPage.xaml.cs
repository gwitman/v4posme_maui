using DevExpress.Maui.CollectionView;
using Posme.Maui.Services;
using Posme.Maui.Services.Repository;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    public partial class MainPage : Shell
    {

        public MainPage()
        {
            InitializeComponent();
        }

        async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current!.MainPage = new LoginPage();
        }

        void OnCloseClicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
        }
    }
}