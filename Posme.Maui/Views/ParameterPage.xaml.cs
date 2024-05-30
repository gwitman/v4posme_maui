using System.Text;
using DevExpress.Maui.Controls;
using DevExpress.Maui.Core;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views;

public partial class ParameterPage : ContentPage
{
    public ParameterPage()
    {
        InitializeComponent();
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
            ((ParameterViewModel)BindingContext).Logo = result.FullPath;
        }
        else
        {
            var stream = await result.OpenReadAsync();
            imageSource = ImageSource.FromStream(() => stream);
            var encoding = Encoding.UTF8;
            using (StreamReader streamReader = new StreamReader(stream, encoding))
            {
                ((ParameterViewModel)BindingContext).Logo = await streamReader.ReadToEndAsync();
            }
        }
        var editorPage = new ImageEditView(imageSource);
        await Navigation.PushAsync(editorPage);
        var cropResult = await editorPage.WaitForResultAsync();
        if (cropResult != null)
        {
            if (!string.IsNullOrEmpty(editorPage.Imagen))
            {
                ((ParameterViewModel)BindingContext).Logo = editorPage.Imagen;
            }
            Preview.Source = cropResult;
        }

        editorPage.Handler.DisconnectHandler();
    }
}