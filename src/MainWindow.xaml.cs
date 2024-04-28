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
        
        private void AddClick (object sender, RoutedEventArgs e)
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
    }
}