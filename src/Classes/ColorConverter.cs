using System.Globalization;
using System.Windows.Data;
using Kalender_Project_FlorianRohat.Pages;

namespace Kalender_Project_FlorianRohat
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Log.log.Information("ColorConverter: Convert function called");
            if (value is DateTime date)
            {
                bool hasToDo = MainPage.toDoCollection?.Any(date) ?? false;
                bool hasEvent = EventView.eventCollection?.EventsList.Any(e =>
                    (e.Date.Date == date.Date) ||
                    (e.EndDate != null && date.Date >= e.Date.Date && date.Date <= e.EndDate.Value.Date)) ?? false;

                if (hasToDo && hasEvent)
                {
                    Log.log.Information("ColorConverter: Day has both event and todo, returning EventState.Both");
                    return EventState.Both;
                }
                else if (hasToDo)
                {
                    Log.log.Information("ColorConverter: Day has only todo, returning EventState.OnlyTodo");
                    return EventState.OnlyTodo;
                }

                else if (hasEvent)
                {
                    Log.log.Information("ColorConverter: Day has only event, returning EventState.OnlyEvent");
                    return EventState.OnlyEvent;
                }
                Log.log.Information("ColorConverter: Day has neither event nor todo, returning EventState.None");
                return EventState.None;
            }
            Log.log.Information("ColorConverter: Day has neither event nor todo, returning EventState.None");
            return EventState.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Log.log.Information("ColorConverter: ConvertBack function called, Returning null");
            return null;
        }
    }
}