using Posme.Maui.Services.Repository;
using iText.IO.Image;
using Unity;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using Image = Microsoft.Maui.Controls.Image;

namespace Posme.Maui.Services.Helpers;

public class PrinterServices
{
    private readonly IRepositoryTbParameterSystem _repositoryTbParameterSystem;
    public string? PrinterName = String.Empty;
    public PrinterServices()
    {
        _repositoryTbParameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
    }


    public async Task SavePdf(string fileName)
    {

        #if ANDROID
		var docsDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
		var filePath = Path.Combine(docsDirectory.AbsoluteFile.Path, fileName);
        #else
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
        #endif
        var paramter =await _repositoryTbParameterSystem.PosMeFindLogo();
        await using var writer = new PdfWriter(filePath);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);
        var header = new Paragraph("MAUI PDF Sample")
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .SetFontSize(20);
        document.Add(header);
        var subheader = new Paragraph("Welcome to .NET Multi-platform App UI")
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .SetFontSize(15);
        document.Add(subheader);
        var ls = new LineSeparator(new SolidLine());
        document.Add(ls);
        var imgStream =Convert.FromBase64String(paramter.Value!);
        var image = new iText.Layout.Element.Image(ImageDataFactory
                .Create(imgStream))
            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

        document.Add(image);
        var footer = new Paragraph("Don't forget to like and subscribe!")
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
            .SetFontColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
            .SetFontSize(14);

        document.Add(footer);
        document.Close();
    }

    private async Task<byte[]> ConvertImageSourceToStreamAsync(string imageName)
    {
        using var ms = new MemoryStream();
        using (var stream = await FileSystem.OpenAppPackageFileAsync(imageName))
            await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}