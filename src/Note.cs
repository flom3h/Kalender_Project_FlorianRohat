namespace Kalender_Project_FlorianRohat;

public class Note
{
    public String Title { get; set; }
    public String Text  { get; set; }
    
    public Note() { }

    public Note(string title, string text)
    {
        Title = title;
        Text = text;
    }
    
}