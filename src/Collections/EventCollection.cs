using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Firebase.Database;
using Kalender_Project_FlorianRohat.UserControls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kalender_Project_FlorianRohat;

public class EventCollection
{
    public List<Event> EventsList { get; set; } = new List<Event>();
    
    public void Add(Event Event)
    {
        Log.log.Information("EventCollection: Add function called, adding event to list");
        EventsList.Add(Event);
    }
    
    public void Serialize(string filename)
    {
        Log.log.Information("EventCollection: Serialize function called, writing events to Json file");
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
        Log.log.Information("EventCollection: Draw function called, drawing events to stackpanel");
        stackPanel.Children.Clear();
        
        StackPanel headerPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(40,15,40,0),
        };

        Image headerImage = new Image
        {
            Source = new BitmapImage(new Uri("../images/eventtImg.png", UriKind.Relative)),
            Width = 32,
            Height = 32,
            Margin = new Thickness(0, 0, 10, 0), 
        };
        headerPanel.Children.Add(headerImage);

        Label headerLabel = new Label
        {
            Content = "Events",
            FontSize = 26,
            Foreground = Brushes.White,
        };
        headerPanel.Children.Add(headerLabel);

        stackPanel.Children.Add(headerPanel);

        foreach (Event events in EventsList.OrderBy(e => e.Date))
        {
            EventItemControl eventItemControl = new EventItemControl();
            eventItemControl.EventText.Text = events.Title;
            eventItemControl.EventDate.Text = events.CombinedDate;
            eventItemControl.Margin = new Thickness(40,2,40,10);
            stackPanel.Children.Add(eventItemControl);
        }
    }
        
}