using Posme.Maui.ViewModels;
using SelectionChangedEventArgs = DevExpress.Maui.Charts.SelectionChangedEventArgs;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((AboutViewModel)BindingContext).OnAppearing(Navigation);
        }

        private void ChartView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}