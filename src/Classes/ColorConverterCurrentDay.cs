using System.Globalization;
using System.Windows.Data;


namespace Kalender_Project_FlorianRohat
{
    public class ColorConverterCurrentDay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Log.log.Information("ColorConverterCurrentDay: Convert function called");
            Log.log.Information("ColorConverterCurrentDay: Checking if day is today");
            if (value is CalendarDay day)
            {
                return day.Date.Date == DateTime.Today;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Log.log.Information("ColorConverterCurrentDay: ConvertBack function called, Returning null");
            return null;
        }
    }
}
