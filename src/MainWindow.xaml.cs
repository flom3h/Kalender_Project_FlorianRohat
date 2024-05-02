using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using Firebase.Database;
using Firebase.Database.Query;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace Kalender_Project_FlorianRohat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ToDoCollection toDoCollection;
        private FirebaseClient firebaseClient;
        public ColorConverter colorConverter;
        public MainWindow()
        {
            InitializeComponent();
            toDoCollection = new ToDoCollection();
            colorConverter = new ColorConverter();
            firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
            DisplayTime();
            Calendar.SelectedDate = DateTime.Today;
            Calendar.DataContext = toDoCollection;
            LoadTodos();

            Application.Current.Properties["ToDoCollection"] = toDoCollection;
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
                    toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value, firebaseClient);
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Datum aus", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value, firebaseClient);
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
                toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value, firebaseClient);
            }
            else
            {
                toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value, firebaseClient);
            }
        }
    }
}