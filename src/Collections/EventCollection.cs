using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Firebase.Database;
using Firebase.Database.Query;
using Kalender_Project_FlorianRohat.UserControls;

namespace Kalender_Project_FlorianRohat;

public class EventCollection
{
    public List<Event> EventsList { get; set; } = new List<Event>();
    
    public void Add(Event Event)
    {
        EventsList.Add(Event);
    }
    
    public void Serialize(string filename)
    {
        using (StreamWriter stream = new StreamWriter(filename))
        {
            foreach (var Event in EventsList)
            {
                string jsonStr = JsonSerializer.Serialize(Event);
                stream.WriteLine(jsonStr);
            }
        }
    }
    
    public void Draw(StackPanel stackPanel, FirebaseClient firebaseClient)
    {
        stackPanel.Children.Clear();
        foreach (Event events in EventsList.OrderBy(e => e.Date))
        {
            EventItemControl eventItemControl = new EventItemControl();
            eventItemControl.EventText.Text = events.Title;
            eventItemControl.EventDate.Text = events.CombinedDate;
            eventItemControl.Margin = new Thickness(2, 0, 2, 10);
            stackPanel.Children.Add(eventItemControl);
        }
    }
        
}