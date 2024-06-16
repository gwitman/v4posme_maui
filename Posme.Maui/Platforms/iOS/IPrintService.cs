using Foundation;
using Posme.Maui;
using Posme.Maui.Services;
using UIKit;

[assembly: Dependency(typeof(PrintService))]
namespace Posme.Maui;

public class PrintService : IPrintService
{
    public void Print(string fileName)
    {
        var printInfo = UIPrintInfo.PrintInfo;
        printInfo.OutputType = UIPrintInfoOutputType.General;

        var printer = UIPrintInteractionController.SharedPrintController;
        printer.PrintInfo = printInfo;
        printer.PrintingItem = NSData.FromFile(fileName);
        printer.ShowsPageRange = true;    
        printer.Present(true, (handler, completed, error) => {
            // Handle completion
        });
    }
}