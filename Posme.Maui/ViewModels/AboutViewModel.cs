using System.Windows.Input;

namespace Posme.Maui.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public const string ViewName = "AboutPage";
        public AboutViewModel()
        {
            Title = "Inicio";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.devexpress.com/maui/"));
        }

        public ICommand OpenWebCommand { get; }
    }
}