using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulerPage : ContentPage
    {
        public SchedulerPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            BindingContext = ViewModel = new ASchedulerViewModel(serviceProvider);
        }

        ASchedulerViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}
