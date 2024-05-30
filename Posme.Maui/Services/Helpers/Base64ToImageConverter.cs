using System.Globalization;

namespace Posme.Maui.Services.Helpers;

public class Base64ToImageConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        var imageBase64 = value as string;
        byte[] imageBytes = System.Convert.FromBase64String(imageBase64);
        return ImageSource.FromStream(() => new MemoryStream(imageBytes));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}