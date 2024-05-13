using System.Windows;


namespace Kalender_Project_FlorianRohat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage());
            MainPage.NotesButtonClicked += DisplayNotes;
            MainPage.CalendarButtonClicked += DisplayCalendar;
        }
        private void DisplayNotes(object sendefr, System.EventArgs e)
        {
            NotesView notesView = new NotesView();
            MainFrame.Navigate(notesView);
        } 
        
        private void DisplayCalendar(object sendefr, System.EventArgs e)
        {
            CalendarMonthView monthView = new CalendarMonthView();
            MainFrame.Navigate(monthView);
        }
    }
}