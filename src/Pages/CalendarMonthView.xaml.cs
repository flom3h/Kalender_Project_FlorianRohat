using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Kalender_Project_FlorianRohat
{
    public partial class CalendarMonthView : Page, INotifyPropertyChanged
    {
        public static ToDoCollection toDoCollection;
        private FirebaseClient firebaseClient;
        private DateTime currentDate;

        public List<string> DaysOfWeek { get; set; }
        private ObservableCollection<CalendarDay> _days;
        public ObservableCollection<CalendarDay> Days
        {
            get { return _days; }
            set
            {
                if (_days != value)
                {
                    _days = value;
                    OnPropertyChanged();
                }
            }
        }
        
        
        public CalendarMonthView()
        {
            InitializeComponent();
            DisplayTime();
            DataContext = this;

            toDoCollection = new ToDoCollection();
            toDoCollection.TodoAdded += OnTodoAdded; // Subscribe to the event
            firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
            
            DaysOfWeek = new List<string> { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };

            currentDate = DateTime.Today;
            _days = new ObservableCollection<CalendarDay>();
            LoadTodosAsync(); // Call the method to load todos asynchronously
            FillDays(currentDate.Year, currentDate.Month, currentDate.Day);
            
        }
        
        private async void LoadTodosAsync()
        {
            await LoadTodos();
        }

        private async Task LoadTodos()
        {
            var todos = await firebaseClient
                .Child("Todo")
                .OnceAsync<ToDo>();
    
            foreach (var todo in todos)
            {
                toDoCollection.Add(todo.Object, todo.Key);
            }
        }
        
        private void FillDays(int year, int month, int day)
        {
            Days.Clear();

            DateTime firstDayOfMonth = new DateTime(year, month, day);
            int offset = (int)firstDayOfMonth.DayOfWeek - 1; // -1 because DayOfWeek starts from Sunday as 0
            offset = offset < 0 ? 6 : offset; // if it's Sunday, set offset to 6

            // Add days from the previous month
            DateTime previousMonth = firstDayOfMonth.AddMonths(-1);
            int daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            for (int i = daysInPreviousMonth - offset + 1; i <= daysInPreviousMonth; i++)
            {
                Days.Add(new CalendarDay(new DateTime(previousMonth.Year, previousMonth.Month, i), 0));
            }

            // Add days of the current month
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime date = new DateTime(year, month, i);
                int tasksCount = toDoCollection.DrawDay(date);
                Days.Add(new CalendarDay(date, tasksCount));
            }

            // Add days from the next month
            DateTime nextMonth = firstDayOfMonth.AddMonths(1);
            int nextDaysToAdd = 42 - Days.Count; // 42 is the total number of cells in a 6x7 grid
            for (int i = 1; i <= nextDaysToAdd; i++)
            {
                Days.Add(new CalendarDay(new DateTime(nextMonth.Year, nextMonth.Month, i), 0));
            }
            

        }
        
        private void Day_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked day
            var button = (Button)sender;
            var clickedDay = (CalendarDay)button.DataContext;

            // Navigate to MainPage and set the selected date
            var mainPage = new MainPage();
            mainPage.Calendar.SelectedDate = clickedDay.Date;
            this.NavigationService.Navigate(mainPage);
        }
        private void OnTodoAdded(object sender, EventArgs e)
        {
            FillDays(currentDate.Year, currentDate.Month, currentDate.Day);
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
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
        private void NextMonth()
        {
            currentDate = currentDate.AddMonths(1);
            FillDays(currentDate.Year, currentDate.Month, currentDate.Day);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            NextMonth();
            OnPropertyChanged(nameof(Days));
        }
        
        
        
        private void PreviousMonth()
        {
            currentDate = currentDate.AddMonths(-1);
            FillDays(currentDate.Year, currentDate.Month, currentDate.Day);
        }
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            PreviousMonth();
            OnPropertyChanged(nameof(Days));
        }
        
        private void DisplayHome(object sender, RoutedEventArgs e)
        {

            var mainPage = new MainPage();
            mainPage.Calendar.SelectedDate = currentDate;
            this.NavigationService.Navigate(mainPage);
            
        }

    }
}