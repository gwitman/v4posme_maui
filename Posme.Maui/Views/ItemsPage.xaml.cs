using System.Diagnostics;
using DevExpress.Maui.Core;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
        }

        private void SearchBar_OnTextChanged(object? sender, EventArgs eventArgs)
        {
            
        }
    }
}