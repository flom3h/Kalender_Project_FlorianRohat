using System.Windows;

namespace Kalender_Project_FlorianRohat;

public partial class AddTodoWindow : Window
{
    public ToDo Todo { get; set; }

    public AddTodoWindow()
    {
        InitializeComponent();
        Todo = new ToDo();
        TodoInput.Text = Todo.Title;
        Todo.IsDone = false;
        TodoDate.SelectedDate = DateTime.Today;

    }
    public AddTodoWindow(ToDo todoToEdit)
    {
        InitializeComponent();
        Todo = todoToEdit;
        TodoInput.Text = Todo.Title;
        TodoDate.SelectedDate = Todo.TodoDate;
    }

    private void CreateClick (object sender, RoutedEventArgs e)
    {
        Log.log.Information("AddTodoWindow: CreateClick() called, creating new todo object");
        try
        {
            Log.log.Information("AddTodoWindow: Check if TodoInput is empty");
            if(string.IsNullOrEmpty(TodoInput.Text) == false && TodoDate.SelectedDate.HasValue)

            {
                Log.log.Information("AddTodoWindow: Check if TodoDate is not in the past");
                if(TodoDate.SelectedDate.Value.Date >= DateTime.Today)
                {
                    Todo.Title = TodoInput.Text;
                    Todo.TodoDate = TodoDate.SelectedDate.Value;
                    DialogResult = true;
                }
                else
                {
                    Log.log.Warning("AddTodoWindow: TodoDate is in the past");
                    MessageBox.Show("Das Datum muss in der Zukunft liegen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Log.log.Warning("AddTodoWindow: TodoInput or TodoDate is empty");
                MessageBox.Show("Gib etwas in die Felder ein", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        catch (Exception ex)
        {
            Log.log.Error(ex, "AddTodoWindow: Error while creating todo");
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}