using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Firebase.Database;
using Firebase.Database.Query;

namespace Kalender_Project_FlorianRohat;

public partial class MainPage : Page
{
    public static ToDoCollection toDoCollection;
    private FirebaseClient firebaseClient;
    public static event EventHandler NotesButtonClicked;
    public static event EventHandler CalendarButtonClicked;
    public MainPage()
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
            if (Calendar.SelectedDate.HasValue)
            {
                toDoCollection.Draw(StackPanelItems, Calendar.SelectedDate.Value, firebaseClient);
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie ein Datum aus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

    private void OnTodoAdded(object sender, EventArgs e)
    {
        LoadTodos();
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
            toDoCollection.Draw(StackPanelItems, Calendar.SelectedDate.Value, firebaseClient);
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
            toDoCollection.Draw(StackPanelItems, Calendar.SelectedDate.Value, firebaseClient);
        }
        else
        {
            toDoCollection.Draw(StackPanelItems, Calendar.SelectedDate.Value, firebaseClient);
        }
    }
    
    private void GoToTodaysTodos(object sender, RoutedEventArgs e)
    {
        Calendar.SelectedDate = DateTime.Today;
        toDoCollection.Draw(StackPanelItems, DateTime.Today, firebaseClient);
    }
    
    private void ShowAllTodos(object sender, RoutedEventArgs e)
    {
        toDoCollection.Draw(StackPanelItems, DateTime.Today, firebaseClient);
    }
}