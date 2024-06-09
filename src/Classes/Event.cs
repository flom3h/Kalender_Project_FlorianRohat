namespace Kalender_Project_FlorianRohat;

public class Event
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public DateTime? EndDate { get; set; }

    public string CombinedDate
    {
        get
        {
            Log.log.Information("Event: Check if event has end date");
            if (EndDate.HasValue)
            {
                Log.log.Information("Event: Event has end date, returning combined date");
                return Date.ToString("dd.MM.yyyy") + " - " + EndDate.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                Log.log.Information("Event: Event has no end date, returning single date");
                return Date.ToString("dd.MM.yyyy");
            }
        }
    }

    public Event(string title, DateTime date, DateTime? endDate = null)
    {
        Title = title;
        Date = date;
        EndDate = endDate;
    }
}