using System.Windows;
using System.Windows.Controls;
using Firebase.Database;
using Firebase.Database.Query;

namespace Kalender_Project_FlorianRohat.Pages;

public partial class EventView : Page
{
    public static EventCollection eventCollection;
    private FirebaseClient firebaseClient;
    public EventView()
    {
        InitializeComponent();
        eventCollection = new EventCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        LoadEvents();
    }
    
    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        if (this.NavigationService.CanGoBack)
        {
            this.NavigationService.GoBack();
        }
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