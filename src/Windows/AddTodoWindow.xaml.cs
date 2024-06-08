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
        try
        {
            if(string.IsNullOrEmpty(TodoInput.Text) == false && TodoDate.SelectedDate.HasValue)

            {
                if(TodoDate.SelectedDate.Value.Date >= DateTime.Today)
                {
                    Todo.Title = TodoInput.Text;
                    Todo.TodoDate = TodoDate.SelectedDate.Value;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Das Datum muss in der Zukunft liegen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Gib etwas in die Felder ein", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}