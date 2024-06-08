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
        public static event EventHandler NotesButtonClicked;
        public static event EventHandler CalendarButtonClicked;
        public static event EventHandler EventButtonClicked; 
        public static event EventHandler AllTodosButtonClicked;
        public static event EventHandler ImportantTodosButtonClicked;
        public static event EventHandler HomeButtonClicked;

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
            firebaseClient = new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
            
            currentDate = DateTime.Today;
            _days = new ObservableCollection<CalendarDay>();
            LoadTodosAsync(); // Call the method to load todos asynchronously
            FillDays(currentDate);
            
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

        private void FillDays(DateTime startDate)
        {
            Days.Clear();

            // Calculate the first day to show (the Monday of the week that includes startDate)
            DateTime firstDayToShow = startDate.AddDays(-(int)startDate.DayOfWeek + (int)DayOfWeek.Monday);

            // Calculate the last day to show (27 days from the first day to show)
            DateTime lastDayToShow = firstDayToShow.AddDays(27);

            // Fill the days from the calculated first day to the last day
            for (DateTime date = firstDayToShow; date <= lastDayToShow; date = date.AddDays(1))
            {
                int tasksCount = toDoCollection.DrawDay(date);
                Days.Add(new CalendarDay(date, tasksCount));
            }
        }

        private void Day_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var clickedDay = (CalendarDay)button.DataContext;

            var mainPage = new MainPage();
            mainPage.Calendar.SelectedDate = clickedDay.Date;
            this.NavigationService.Navigate(mainPage);
        }

        private void OnTodoAdded(object sender, EventArgs e)
        {
            FillDays(currentDate);
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
            DateTime lastDayDisplayed = Days.Last().Date;
            currentDate = lastDayDisplayed.AddDays(1);
            FillDays(currentDate);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            NextMonth();
            OnPropertyChanged(nameof(Days));
        }

        private void PreviousMonth()
        {
            currentDate = currentDate.AddDays(-28);
            FillDays(currentDate);
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            PreviousMonth();
            OnPropertyChanged(nameof(Days));
        }

        private void DisplayHome(object sender, RoutedEventArgs e)
        {
            HomeButtonClicked?.Invoke(this, EventArgs.Empty);
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
    
        private void DisplayAllTodos(object sender, RoutedEventArgs e)
        {
            AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    
        private void DisplayImportantTodos(object sender, RoutedEventArgs e)
        {
            ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}