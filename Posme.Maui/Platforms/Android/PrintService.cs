using Android.Content;
using Android.OS;
using Android.Print;
using Java.IO;
using Posme.Maui;
using Posme.Maui.Helpers;
using Posme.Maui.Services;

[assembly: Dependency(typeof(PrintService))]
namespace Posme.Maui;

public class PrintService : IPrintService
{
    public void Print(string filePath)
    {
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        if (fileStream.CanSeek)
            fileStream.Position = 0;
        //Save the stream to the created file
        using (var dest = System.IO.File.OpenWrite(filePath))
        {
            fileStream.CopyTo(dest);
        }

        if (Platform.CurrentActivity == null)
            return;

        var printManager = (PrintManager)Platform.CurrentActivity.GetSystemService(Context.PrintService);

        // Now we can use the preexisting print helper class
        var utility = new PrintUtility(filePath);

        printManager?.Print(filePath, utility, null);
    }
}

public class PdfPrintDocumentAdapter : PrintDocumentAdapter
{
    public string PrintFileName { get; set; }

    public PdfPrintDocumentAdapter(string printFileName)
    {
        PrintFileName = printFileName;
    }

    public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, PrintDocumentAdapter.LayoutResultCallback callback, Bundle extras)
    {
        if (cancellationSignal.IsCanceled)
        {
            callback.OnLayoutCancelled();
            return;
        }

        var pdi = new PrintDocumentInfo.Builder(PrintFileName).SetContentType(PrintContentType.Document).Build();

        callback.OnLayoutFinished(pdi, true);
    }

    public override void OnWrite(Android.Print.PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, PrintDocumentAdapter.WriteResultCallback callback)
    {
        InputStream input = null;
        OutputStream output = null;

        try
        {
            input = new FileInputStream(PrintFileName);
            output = new FileOutputStream(destination.FileDescriptor);

            var buf = new byte[1024];
            int bytesRead;

            while ((bytesRead = input.Read(buf)) > 0)
            {
                output.Write(buf, 0, bytesRead);
            }

            callback.OnWriteFinished(new[] { Android.Print.PageRange.AllPages });

        }
        catch (Java.IO.FileNotFoundException ee)
        {
            //Catch
        }
        catch (Exception e)
        {
            //Catch
        }
        finally
        {
            try
            {
                input?.Close();
                output?.Close();
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }
        }
    }
}