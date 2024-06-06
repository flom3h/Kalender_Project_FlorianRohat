
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Kalender_Project_FlorianRohat.Pages;

namespace Kalender_Project_FlorianRohat
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                bool hasToDo = MainPage.toDoCollection?.Any(date) ?? false;
                
                bool hasEvent = EventView.eventCollection?.EventsList.Any(e =>
                    (e.Date.Date == date.Date) ||
                    (e.EndDate != null && date.Date >= e.Date.Date && date.Date <= e.EndDate.Value.Date)) ?? false;
                
                if (hasToDo && hasEvent)
                    return Brushes.Purple;
                else if (hasToDo)
                    return Brushes.Green;
                else if (hasEvent)
                    return Brushes.Red;

                return Brushes.White;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}