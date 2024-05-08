using System.Windows.Controls;

namespace Kalender_Project_FlorianRohat;

public partial class CalendarMonthView : Page
{
    public List<string> DaysOfWeek { get; set; }
    public List<int> Days { get; set; }
    public string Today { get; set; }

    
    public static ToDoCollection toDoCollection;

    public CalendarMonthView()
    {
        InitializeComponent();

        // Initialize days of the week
        DaysOfWeek = new List<string> { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };
        DateTime currentDate = DateTime.Today;
        Today = $"{currentDate.DayOfWeek.ToString()} {currentDate.Day}";
        Days = new List<int>();
        for (int i = 1; i <= 31; i++)
        {
            Days.Add(i);
        }

        DataContext = this;
    }

}