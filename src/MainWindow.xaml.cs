using System.Windows;
using System.Windows.Threading;

namespace Kalender_Project_FlorianRohat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ToDoCollection toDoCollection = new ToDoCollection();
        public MainWindow()
        {
            InitializeComponent();
            DisplayTime();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            AddTodoWindow addTodoWindow = new AddTodoWindow();
            if (addTodoWindow.ShowDialog() == true)
            {
                toDoCollection.Add(addTodoWindow.Todo);
                toDoCollection.Draw(stackPanel);
            }
        }

        private DateTime? GetDateTime()
        {
            return Calendar.SelectedDate ?? DateTime.Today;
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

        private void EditClick(object sender, RoutedEventArgs e)
        {
           
            if (toDoCollection.ToDoList.Count() > 0)
            {
                ToDo todoToEdit = toDoCollection.ToDoList[0];

                // Öffne das Fenster zur Bearbeitung mit den Informationen des zu bearbeitenden ToDo-Elements
                AddTodoWindow editTodoWindow = new AddTodoWindow(todoToEdit);

                // Wenn der Benutzer auf "Speichern" klickt
                if (editTodoWindow.ShowDialog() == true)
                {
                    // Aktualisiere das ToDo-Element
                    toDoCollection.Edit(todoToEdit, editTodoWindow.Todo.Title, editTodoWindow.Todo.TodoDate);
                    // Zeichne die Liste erneut
                    toDoCollection.Draw(stackPanel);
                }
            }   
            
        }
    }
}