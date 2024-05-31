using System.Text;
using DevExpress.Maui.Controls;
using Posme.Maui.ViewModels;
using Debug = System.Diagnostics.Debug;

namespace Posme.Maui.Views;

public partial class ParameterPage : ContentPage
{
    private readonly ParameterViewModel _viewModel;

    public ParameterPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new ParameterViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing(Navigation);
        _viewModel.LoadValuesDefault();
    }

    private void ClosePopup_Clicked(object? sender, EventArgs e)
    {
        Popup.IsOpen = false;
    }

    private void ImageTapped(object sender, EventArgs e)
    {
        BottomSheet.State = BottomSheetState.HalfExpanded;
    }

    private async void DeletePhotoClicked(object sender, EventArgs args)
    {
        await Dispatcher.DispatchAsync(() =>
        {
            BottomSheet.State = BottomSheetState.Hidden;
            EditControl.IsVisible = false;
            Preview.Source = null;
        });
    }

    private async void SelectPhotoClicked(object sender, EventArgs args)
    {
        var photo = await MediaPicker.PickPhotoAsync();
        await ProcessResult(photo);
        EditControl.IsVisible = true;
    }

    private async void TakePhotoClicked(object sender, EventArgs args)
    {
        if (!MediaPicker.Default.IsCaptureSupported)
            return;

        var photo = await MediaPicker.Default.CapturePhotoAsync();
        await ProcessResult(photo);
        EditControl.IsVisible = true;
    }

    private async Task ProcessResult(FileResult? result)
    {
        await Dispatcher.DispatchAsync(() => { BottomSheet.State = BottomSheetState.Hidden; });

        if (result == null)
            return;

        ImageSource imageSource;
        if (Path.IsPathRooted(result.FullPath))
        {
            imageSource = ImageSource.FromFile(result.FullPath);
        }
        else
        {
            var encoding = Encoding.UTF8;
            await using var stream = await result.OpenReadAsync();
            var streamReader = new StreamReader(stream, encoding);
            imageSource = ImageSource.FromStream(() => streamReader.BaseStream);
        }

        var editorPage = new ImageEditView(imageSource);
        await Navigation.PushAsync(editorPage);
        var cropResult = await editorPage.WaitForResultAsync();
        if (cropResult != null)
        {
            Preview.Source = cropResult;
        }

        editorPage.Handler!.DisconnectHandler();
    }

    private void RefreshView_OnRefreshing(object? sender, EventArgs e)
    {
        OnAppearing();
        Preview.Source = _viewModel.ShowImage;
        Debug.WriteLine(Preview);
    }
}