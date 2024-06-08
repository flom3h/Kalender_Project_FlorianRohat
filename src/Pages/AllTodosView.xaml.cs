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
        InitializeComponent();
        toDoCollection = new ToDoCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        Calendar.SelectedDate = DateTime.Today;
        Calendar.DataContext = toDoCollection;
        LoadTodos();
    }

    private async void AddClick(object sender, RoutedEventArgs e)
    {
        AddTodoWindow addTodoWindow = new AddTodoWindow();
        if (addTodoWindow.ShowDialog() == true)
        {
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
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Title = "Save ToDo List";
        saveFileDialog.Filter = "JSON File|*.json";
        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        saveFileDialog.Filter = "JSON File|*.json";
        
        if (saveFileDialog.ShowDialog() == true)
        {
            string fileName = saveFileDialog.FileName;
            toDoCollection.Serialize(fileName);

        }
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
    
    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        HomeButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayImportantTodos(object sender, RoutedEventArgs e)
    {
        ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private async void LoadTodos()
    {
        var todos = await firebaseClient
            .Child("Todo")
            .OnceAsync<ToDo>();
        
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
            MessageBox.Show("Bitte wählen Sie ein Datum aus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private void DateChange(object? sender, SelectionChangedEventArgs e)
    {
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