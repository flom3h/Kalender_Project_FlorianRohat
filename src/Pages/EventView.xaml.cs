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
        Log.log.Information("EventView opened");
        InitializeComponent();
        eventCollection = new EventCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        LoadEvents();
        Log.log.Information("EventView initialized, components loaded");
    }
    
    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        Log.log.Information("EventView: Home button clicked, changing to MainPage");
        HomeButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    private void DisplayNotes(object sender, RoutedEventArgs e)
    {
        Log.log.Information("EventView: Home button clicked, changing to NotesView");
        NotesButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayCalendar(object sender, RoutedEventArgs e)
    {
        Log.log.Information("EventView: Calendar button clicked, changing to CalendarView");
        CalendarButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void DisplayEvents(object sender, RoutedEventArgs e)
    {
        Log.log.Information("EventView: Event button clicked, changing to EventView");
        EventButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("EventView: AllTodos button clicked, changing to AllTodosView");
        AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayImportantTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("EventView: ImportantTodos button clicked, changing to ImportantView");
        ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayTime()
    {
        Log.log.Information("EventView: DisplayTime function called, starting timer to display time");
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
        Log.log.Information("EventView: LoadEvents function called, loading events from firebase");
        var Events = await firebaseClient
            .Child("Feiertage")
            .OnceAsync<Event>();
        Log.log.Information("EventView: Loaded Events from Firebase");
        
        var Holidays = await firebaseClient
            .Child("Ferien")
            .OnceAsync<Event>();
        Log.log.Information("EventView: Loaded Holidays from Firebase");
        
        Log.log.Information("EventView: adding events to eventCollection");
        foreach (var Event in Events)
        {
            eventCollection.Add(Event.Object);
        }

        Log.log.Information("EventView: adding holidays to eventCollection");
        foreach (var Holiday in Holidays)
        {
            eventCollection.Add(Holiday.Object);
        }
        Log.log.Information("EventView: Drawing events to stackPanel");
        eventCollection.Draw(StackPanelItem, firebaseClient);

        /*else
        {
            MessageBox.Show("Keine Events gefunden" , "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }*/
    }
}