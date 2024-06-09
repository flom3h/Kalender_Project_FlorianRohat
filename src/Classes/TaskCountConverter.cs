using System.Globalization;
using System.Windows.Data;

namespace Kalender_Project_FlorianRohat
{
    public class TaskCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Log.log.Information("TaskCountConverter: Convert function called");
            if (value is int tasksCount)
            {
                if (tasksCount == 0)
                {
                    Log.log.Information("TaskCountConverter: No tasks, returning empty string");
                    return string.Empty;
                }
                else if (tasksCount == 1)
                {
                    Log.log.Information("TaskCountConverter: One task, returning string");
                    return "1 Aufgabe";
                }
                else
                {
                    Log.log.Information("TaskCountConverter: Multiple tasks, returning string");
                    return $"{tasksCount} Aufgaben";
                }
            }
            Log.log.Information("TaskCountConverter: No tasks, returning empty string");
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Log.log.Information("TaskCountConverter: ConvertBack function called, Returning null");
            return null;
        }
    }
}
