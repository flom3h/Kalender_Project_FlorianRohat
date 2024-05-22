namespace Kalender_Project_FlorianRohat
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public int TasksCount { get; set; }
        
        public CalendarDay(DateTime date, int tasksCount)
        {
            Date = date;
            TasksCount = tasksCount;
        }
        
        public override string ToString()
        {
            return Date.Day.ToString();

        }
    }
}

