using System.Globalization;
using System.Windows.Data;

namespace Kalender_Project_FlorianRohat
{
    public class TaskCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int tasksCount)
            {
                if (tasksCount == 0)
                {
                    return string.Empty;
                }
                else if (tasksCount == 1)
                {
                    return "1 Aufgabe";
                }
                else
                {
                    return $"{tasksCount} Aufgaben";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
