using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Firebase.Database;
using Firebase.Database.Query;


namespace Kalender_Project_FlorianRohat.Pages;

public partial class ImportantView : Page
{
    public static ToDoCollection toDoCollection;
    private FirebaseClient firebaseClient;
    public static event EventHandler NotesButtonClicked;
    public static event EventHandler CalendarButtonClicked;
    public static event EventHandler EventButtonClicked; 
    public static event EventHandler AllTodosButtonClicked;
    public static event EventHandler ImportantTodosButtonClicked;
    public static event EventHandler HomeButtonClicked;

    public ImportantView()
    {
        Log.log.Information("ImportantView opened");
        InitializeComponent();
        toDoCollection = new ToDoCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        Calendar.SelectedDate = DateTime.Today;
        Calendar.DataContext = toDoCollection;
        LoadTodos();
        Log.log.Information("ImportantView initialized, components loaded");
    }

    private async void AddClick(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: Add button clicked, opening AddTodoWindow");
        AddTodoWindow addTodoWindow = new AddTodoWindow();
        Log.log.Information("ImportantView: Check if AddTodoWindow is true");
        if (addTodoWindow.ShowDialog() == true)
        {
            var addedTodo = await firebaseClient
                .Child("Todo")
                .PostAsync(addTodoWindow.Todo);
            Log.log.Information("ImportantView: added todo to firebase database");
            toDoCollection.Add(addTodoWindow.Todo, addedTodo.Key);
            Log.log.Information("ImportantView: added todo to collection");
            toDoCollection.DrawImportantTodos(StackPanelItems, firebaseClient);
            Calendar.UpdateLayout();
        }
    }
    
    private void SaveClick(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: Save button clicked, opening SaveFileDialog");
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Title = "Save ToDo List";
        saveFileDialog.Filter = "JSON File|*.json";
        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        saveFileDialog.Filter = "JSON File|*.json";
        
        Log.log.Information("ImportantView: Check if SaveFileDialog is true");
        if (saveFileDialog.ShowDialog() == true)
        {
            Log.log.Information("ImportantView: SaveFileDialog is true, writing to json file");
            string fileName = saveFileDialog.FileName;
            toDoCollection.Serialize(fileName);

        }
    }
    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: Home button clicked, changing to MainPage");
        HomeButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    private void DisplayNotes(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: Home button clicked, changing to NotesView");
        NotesButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayCalendar(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: Calendar button clicked, changing to CalendarView");
        CalendarButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void DisplayEvents(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: Event button clicked, changing to EventView");
        EventButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: AllTodos button clicked, changing to AllTodosView");
        AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayImportantTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("ImportantView: ImportantTodos button clicked, changing to ImportantView");
        ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private async void LoadTodos()
    {
        Log.log.Information("ImportantView: Loading todos from firebase database");
        var todos = await firebaseClient
            .Child("Todo")
            .OnceAsync<ToDo>();
        Log.log.Information("ImportantView: Loaded todos from firebase database");
        
        Log.log.Information("ImportantView: Adding todos to collection");
        foreach (var todo in todos)
        {
            toDoCollection.Add(todo.Object, todo.Key);
        }
        if (Calendar.SelectedDate.HasValue)
        {
            Log.log.Information("ImportantView: Drawing all important todos to stackpanel");
            toDoCollection.DrawImportantTodos(StackPanelItems, firebaseClient);
        }
        else
        {
            Log.log.Warning("ImportantView: Error, no date selected");
            MessageBox.Show("Bitte wählen Sie ein Datum aus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DisplayTime()
    {
        Log.log.Information("ImportantView: DisplayTime function called, starting timer to display time");
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, e) =>
        {
            DateDisplay.Content = DateTime.Now.ToString("hh:mm:ss tt");
        };
        timer.Start();
    }
    
}