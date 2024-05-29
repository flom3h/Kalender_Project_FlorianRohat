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
            if (EndDate.HasValue)
            {
                return Date.ToString("dd.MM.yyyy") + " - " + EndDate.Value.ToString("dd.MM.yyyy");
            }
            else
            {
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