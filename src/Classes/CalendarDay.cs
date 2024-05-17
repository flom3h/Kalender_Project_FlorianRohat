namespace Kalender_Project_FlorianRohat
{
    public class CalendarDay
    {
        public DateTime Days { get; set; }
        public int ToDos { get; set; }
        
        public CalendarDay(DateTime days, int toDos)
        {
            Days = days;
            ToDos = toDos;
        }
        
        public override string ToString()
        {
            if (ToDos == 1)
            {
                return Days.Day.ToString() + "\n" + $"{ToDos} Aufgabe";
            }
            else if (ToDos > 1)
            {
                return Days.Day.ToString() + "\n" + $"{ToDos} Aufgaben";
            }
            else
            {
                return Days.Day.ToString();
            }
        }
    }
}

