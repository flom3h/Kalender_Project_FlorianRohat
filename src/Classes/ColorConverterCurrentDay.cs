using System;
using System.Globalization;
using System.Windows.Data;


namespace Kalender_Project_FlorianRohat
{
    public class ColorConverterCurrentDay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CalendarDay day)
            {
                return day.Date.Date == DateTime.Today;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
