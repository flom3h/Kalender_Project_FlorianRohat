using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Firebase.Database;

namespace Kalender_Project_FlorianRohat.Pages;

public partial class EventView : Page
{
    public static EventCollection eventCollection;
    private FirebaseClient firebaseClient;
    public static event EventHandler NotesButtonClicked;
    public static event EventHandler CalendarButtonClicked;
    public static event EventHandler EventButtonClicked; 
    public static event EventHandler AllTodosButtonClicked;
    public static event EventHandler ImportantTodosButtonClicked;
    public static event EventHandler HomeButtonClicked;
    public EventView()
    {
        InitializeComponent();
        eventCollection = new EventCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        LoadEvents();
    }
    
    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        HomeButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    private void DisplayNotes(object sender, RoutedEventArgs e)
    {
        NotesButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayCalendar(object sender, RoutedEventArgs e)
    {
        CalendarButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void DisplayEvents(object sender, RoutedEventArgs e)
    {
        EventButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayImportantTodos(object sender, RoutedEventArgs e)
    {
        ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayTime()
    {
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, e) =>
        {
            DateDisplay.Content = DateTime.Now.ToString("hh:mm:ss tt");
        };
        timer.Start();
    }
    
    private async void LoadEvents()
    {
        var Events = await firebaseClient
            .Child("Feiertage")
            .OnceAsync<Event>();
        
        var Holidays = await firebaseClient
            .Child("Ferien")
            .OnceAsync<Event>();
        
        foreach (var Event in Events)
        {
            eventCollection.Add(Event.Object);
        }

        foreach (var Holiday in Holidays)
        {
            eventCollection.Add(Holiday.Object);
        }
        eventCollection.Draw(stackPanel, firebaseClient);

        /*else
        {
            MessageBox.Show("Keine Events gefunden" , "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }*/
    }
}