using Posme.Maui.ViewModels.Abonos;

namespace Posme.Maui.Views.Abonos;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PrintViewPage : ContentPage
{
    public PrintViewPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((PrintViewViewModel)BindingContext).OnAppearing();
    }

    private void MenuItem_OnClicked(object? sender, EventArgs e)
    {
        PdfViewer.PrintDocument();
        PdfViewer.PrintDocument();
    }
}