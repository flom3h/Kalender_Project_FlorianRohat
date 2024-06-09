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
            Log.log.Information("CalendarMonthView opened");
            InitializeComponent();
            DisplayTime();
            DataContext = this;

            toDoCollection = new ToDoCollection();
            toDoCollection.TodoAdded += OnTodoAdded;
            firebaseClient = new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
            currentDate = DateTime.Today;
            _days = new ObservableCollection<CalendarDay>();
            LoadTodosAsync();
            FillDays(currentDate);
            Log.log.Information("CalendarMonthView initialized, components loaded");
        }
        
        private async void LoadTodosAsync()
        {
            Log.log.Information("CalendarMonthView: LoadTodosAsync function called");
            await LoadTodos();
        }

        private async Task LoadTodos()
        {
            Log.log.Information("CalendarMonthView: LoadTodos function called, loading todos from firebase");
            var todos = await firebaseClient
                .Child("Todo")
                .OnceAsync<ToDo>();

            Log.log.Information("CalendarMonthView: Adding todos to collection");
            foreach (var todo in todos)
            {
                toDoCollection.Add(todo.Object, todo.Key);
            }
        }

        private void FillDays(DateTime startDate)
        {
            Log.log.Information("CalendarMonthView: FillDays function called, filling days with todos");
            Days.Clear();

            DateTime firstDayToShow = startDate.DayOfWeek == DayOfWeek.Sunday
                ? startDate.AddDays(-6)
                : startDate.AddDays(-(int)startDate.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime lastDayToShow = firstDayToShow.AddDays(27);
            
            Log.log.Information("CalendarMonthView: Drawing each day");
            Log.log.Information("TodoCollection: DrawDay function called, returning number of todos for the date");
            for (DateTime date = firstDayToShow; date <= lastDayToShow; date = date.AddDays(1))
            {
                int tasksCount = toDoCollection.DrawDay(date);
                Days.Add(new CalendarDay(date, tasksCount));
            }
        }

        private void Day_Click(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: DayElement clicked, navigating to MainPage");
            var button = (Button)sender;
            var clickedDay = (CalendarDay)button.DataContext;
            var mainPage = new MainPage();
            mainPage.Calendar.SelectedDate = clickedDay.Date;
            this.NavigationService.Navigate(mainPage);
        }

        private void OnTodoAdded(object sender, EventArgs e)
        {
            Log.log.Information("CalendarMonthView: OnTodoAdded function called, calling FillDays function");
            FillDays(currentDate);
        }

        private async void AddClick(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: Add button clicked, opening AddTodoWindow");
            AddTodoWindow addTodoWindow = new AddTodoWindow();
            Log.log.Information("CalendarMonthView: Checking if AddTodoWindow is true");
            if (addTodoWindow.ShowDialog() == true)
            {
                Log.log.Information("CalendarMonthView: AddTodoWindow is true, adding todo to firebase");
                var addedTodo = await firebaseClient
                    .Child("Todo")
                    .PostAsync(addTodoWindow.Todo);
                Log.log.Information("CalendarMonthView: Adding todo to collection");
                toDoCollection.Add(addTodoWindow.Todo, addedTodo.Key);
            }
        }

        private void DisplayTime()
        {
            Log.log.Information("CalendarMonthView: DisplayTime function called, starting timer to display time");
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
            Log.log.Information("CalendarMonthView: OnPropertyChanged function called, updating property");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NextMonth()
        {
            Log.log.Information("CalendarMonthView: NextMonth function called, drawing next month");
            DateTime lastDayDisplayed = Days.Last().Date;
            currentDate = lastDayDisplayed.AddDays(1);
            FillDays(currentDate);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: NextMonth button clicked, changing to next month");
            NextMonth();
            OnPropertyChanged(nameof(Days));
        }

        private void PreviousMonth()
        {
            Log.log.Information("CalendarMonthView: PreviousMonth function called, drawing previous month");
            currentDate = currentDate.AddDays(-28);
            FillDays(currentDate);
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: PreviousMonth button clicked, changing to previous month");
            PreviousMonth();
            OnPropertyChanged(nameof(Days));
        }

        private void DisplayHome(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: Home button clicked, changing to MainPage");
            HomeButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void DisplayNotes(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: Home button clicked, changing to NotesView");
            NotesButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    
        private void DisplayCalendar(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: Calendar button clicked, changing to CalendarView");
            CalendarButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void DisplayEvents(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: Event button clicked, changing to EventView");
            EventButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    
        private void DisplayAllTodos(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: AllTodos button clicked, changing to AllTodosView");
            AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    
        private void DisplayImportantTodos(object sender, RoutedEventArgs e)
        {
            Log.log.Information("CalendarMonthView: ImportantTodos button clicked, changing to ImportantView");
            ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}