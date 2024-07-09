using System.ComponentModel;
using System.Diagnostics;
using Posme.Maui.Services.Helpers;
using Posme.Maui.Services.SystemNames;
namespace Posme.Maui.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            Navigated += (sender, e) =>
            {
                var current = e.Current?.Location?.ToString();
                Debug.WriteLine($"Current tab: {current}");
            };
        }
        
        void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current!.MainPage = new LoginPage();
        }
    }
}