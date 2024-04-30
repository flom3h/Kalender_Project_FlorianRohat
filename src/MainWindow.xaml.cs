using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;

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
                if (Calendar.SelectedDate.HasValue)
                {
                    toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value);
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
                toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value);
            }
            else
            {
                toDoCollection.Draw(stackPanel, Calendar.SelectedDate.Value);
            }
        }
    }
}