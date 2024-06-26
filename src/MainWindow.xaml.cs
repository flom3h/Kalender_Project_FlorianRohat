﻿using System.Windows;
using Kalender_Project_FlorianRohat.Pages;


namespace Kalender_Project_FlorianRohat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Log.log.Information("Application started");
            InitializeComponent();
            MainFrame.Navigate(new MainPage());
            MainPage.NotesButtonClicked += DisplayNotes;
            MainPage.CalendarButtonClicked += DisplayCalendar;
            MainPage.EventButtonClicked += DisplayEvents;
            MainPage.AllTodosButtonClicked += DisplayAllTodos;
            MainPage.ImportantTodosButtonClicked += DisplayImportantTodos;
            MainPage.HomeButtonClicked += DisplayHome;
            AllTodosView.HomeButtonClicked += DisplayHome;
            AllTodosView.NotesButtonClicked += DisplayNotes;
            AllTodosView.CalendarButtonClicked += DisplayCalendar;
            AllTodosView.EventButtonClicked += DisplayEvents;
            AllTodosView.AllTodosButtonClicked += DisplayAllTodos;
            AllTodosView.ImportantTodosButtonClicked += DisplayImportantTodos;
            ImportantView.HomeButtonClicked += DisplayHome;
            ImportantView.NotesButtonClicked += DisplayNotes;
            ImportantView.CalendarButtonClicked += DisplayCalendar;
            ImportantView.EventButtonClicked += DisplayEvents;
            ImportantView.AllTodosButtonClicked += DisplayAllTodos;
            ImportantView.ImportantTodosButtonClicked += DisplayImportantTodos;
            EventView.HomeButtonClicked += DisplayHome;
            EventView.NotesButtonClicked += DisplayNotes;
            EventView.CalendarButtonClicked += DisplayCalendar;
            EventView.EventButtonClicked += DisplayEvents;
            EventView.AllTodosButtonClicked += DisplayAllTodos;
            EventView.ImportantTodosButtonClicked += DisplayImportantTodos;
            NotesView.HomeButtonClicked += DisplayHome;
            NotesView.NotesButtonClicked += DisplayNotes;
            NotesView.CalendarButtonClicked += DisplayCalendar;
            NotesView.EventButtonClicked += DisplayEvents;
            NotesView.AllTodosButtonClicked += DisplayAllTodos;
            NotesView.ImportantTodosButtonClicked += DisplayImportantTodos;
            CalendarMonthView.HomeButtonClicked += DisplayHome;
            CalendarMonthView.NotesButtonClicked += DisplayNotes;
            CalendarMonthView.CalendarButtonClicked += DisplayCalendar;
            CalendarMonthView.EventButtonClicked += DisplayEvents;
            CalendarMonthView.AllTodosButtonClicked += DisplayAllTodos;
            CalendarMonthView.ImportantTodosButtonClicked += DisplayImportantTodos;
            Log.log.Information("All components loaded successfully");
        }
        private void DisplayNotes(object sendefr, System.EventArgs e)
        {
            NotesView notesView = new NotesView();
            MainFrame.Navigate(notesView);
            Log.log.Information("Displaying NotesView");
        } 
        
        private void DisplayCalendar(object sendefr, System.EventArgs e)
        {
            CalendarMonthView monthView = new CalendarMonthView();
            MainFrame.Navigate(monthView);
            Log.log.Information("Displaying CalendarView");
        }
        
        private void DisplayEvents (object sendefr, System.EventArgs e)
        {
            EventView eventView = new EventView();
            MainFrame.Navigate(eventView);
            Log.log.Information("Displaying EventView");
        }
        
        private void DisplayAllTodos (object sendefr, System.EventArgs e)
        {
            AllTodosView allTodosPage = new AllTodosView();
            MainFrame.Navigate(allTodosPage);
            Log.log.Information("Displaying AllTodosView");
        }
        
        private void DisplayImportantTodos (object sendefr, System.EventArgs e)
        {
            ImportantView importantView = new ImportantView();
            MainFrame.Navigate(importantView);
            Log.log.Information("Displaying ImportantView");
        }
        
        private void DisplayHome (object sendefr, System.EventArgs e)
        {
            MainPage mainPage = new MainPage();
            MainFrame.Navigate(mainPage);
            Log.log.Information("Displaying MainPage");
        }
    }
}