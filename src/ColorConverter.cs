using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Kalender_Project_FlorianRohat
{

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (parameter is ToDoCollection toDoCollection)
            {
                // Check if there are tasks for this date
                if (TasksExistForDate(date, toDoCollection))
                {
                    MessageBox.Show($"Tasks exist for {date}, changing color to red");
                    return Brushes.Red;
                }
            }
            MessageBox.Show($"No tasks for {date}, keeping default color");
            return Brushes.Blue;
        }


        private static bool TasksExistForDate(DateTime date, ToDoCollection toDoCollection)
        {
            bool tasksExist = toDoCollection.ToDoList.Any(todo => todo.TodoDate.Date == date.Date);
            MessageBox.Show($"Tasks exist for {date}: {tasksExist}");
            return tasksExist;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
