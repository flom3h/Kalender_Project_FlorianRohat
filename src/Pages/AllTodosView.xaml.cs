using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Firebase.Database;
using Firebase.Database.Query;

namespace Kalender_Project_FlorianRohat.Pages;

public partial class AllTodosView : Page
{
    public static ToDoCollection toDoCollection;
    private FirebaseClient firebaseClient;
    public static event EventHandler NotesButtonClicked;
    public static event EventHandler CalendarButtonClicked;
    public static event EventHandler EventButtonClicked; 
    public static event EventHandler AllTodosButtonClicked;
    public static event EventHandler ImportantTodosButtonClicked;
    public static event EventHandler HomeButtonClicked;
    
    public AllTodosView()
    {
        Log.log.Information("AllTodosView opened");
        InitializeComponent();
        toDoCollection = new ToDoCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        Calendar.SelectedDate = DateTime.Today;
        Calendar.DataContext = toDoCollection;
        LoadTodos();
        Log.log.Information("AllTodosView initialized, components loaded");
    }

    private async void AddClick(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: Add button clicked, opening AddTodoWindow");
        AddTodoWindow addTodoWindow = new AddTodoWindow();
        Log.log.Information("Checking if AddTodoWindow is true");
        if (addTodoWindow.ShowDialog() == true)
        {
            Log.log.Information("AddTodoWindow is true, adding todo to firebase");
            var addedTodo = await firebaseClient
                .Child("Todo")
                .PostAsync(addTodoWindow.Todo);
            toDoCollection.Add(addTodoWindow.Todo, addedTodo.Key);
            toDoCollection.DrawAllTodos(StackPanelItems, firebaseClient);
            Calendar.UpdateLayout();
        }
    }
    
    private void SaveClick(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: Save button clicked, opening SaveFileDialog");
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Title = "Save ToDo List";
        saveFileDialog.Filter = "JSON File|*.json";
        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        saveFileDialog.Filter = "JSON File|*.json";
        Log.log.Information("Checking if SaveFileDialog is true");
        if (saveFileDialog.ShowDialog() == true)
        {
            Log.log.Information("SaveFileDialog is true, writing todo list to json file");
            string fileName = saveFileDialog.FileName;
            toDoCollection.Serialize(fileName);
        }
    }
    
    private void DisplayNotes(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: Home button clicked, changing to NotesView");
        NotesButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayCalendar(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: Calendar button clicked, changing to CalendarView");
        CalendarButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void DisplayEvents(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: Event button clicked, changing to EventView");
        EventButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: Home button clicked, changing to MainPage");
        HomeButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: AllTodos button clicked, changing to AllTodosView");
        AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayImportantTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AllTodosView: ImportantTodos button clicked, changing to ImportantView");
        ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private async void LoadTodos()
    {
        Log.log.Information("AllTodosView: LoadTodos function called, Loading todos from firebase");
        var todos = await firebaseClient
            .Child("Todo")
            .OnceAsync<ToDo>();
        
        Log.log.Information("AllTodosView: Adding todos to ToDoCollection");
        foreach (var todo in todos)
        {
            toDoCollection.Add(todo.Object, todo.Key);
        }
        if (Calendar.SelectedDate.HasValue)
        {
            toDoCollection.DrawAllTodos(StackPanelItems, firebaseClient);
        }
        else
        {
            Log.log.Warning("AllTodosView: No date selected, showing error message");
            MessageBox.Show("Bitte wählen Sie ein Datum aus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void DisplayTime()
    {
        Log.log.Information("AllTodosView: DisplayTime function called, starting timer to display time");
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, e) =>
        {
            DateDisplay.Content = DateTime.Now.ToString("hh:mm:ss tt");
        };
        timer.Start();
    }

    private void DateChange(object? sender, SelectionChangedEventArgs e)
    {
        Log.log.Information("AllTodosView: DateChange function called, changing date");
        if (Calendar.SelectedDate.HasValue)
        {
            toDoCollection.DrawAllTodos(StackPanelItems, firebaseClient);
        }
        else
        {
            toDoCollection.DrawAllTodos(StackPanelItems, firebaseClient);
        }
    }
    
}