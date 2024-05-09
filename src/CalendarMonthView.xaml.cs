using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Kalender_Project_FlorianRohat
{
    public partial class CalendarMonthView : Page
    {
        public List<string> DaysOfWeek { get; set; }
        private List<string> _days;
        public List<string> Days
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
        private DateTime currentDate;


        public CalendarMonthView()
        {
            InitializeComponent();

            // Initialize days of the week
            DaysOfWeek = new List<string> { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };

            currentDate = DateTime.Today;
            _days = new List<string>();
            FillDays(currentDate.Year, currentDate.Month);

            DataContext = this;
        }

        private void FillDays(int year, int month)
        {
            Days.Clear();
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int offset = (int)firstDayOfMonth.DayOfWeek - 1; // -1 because DayOfWeek starts from Sunday as 0
            offset = offset < 0 ? 6 : offset; // if it's Sunday, set offset to 6

            // Add days from the previous month
            DateTime previousMonth = firstDayOfMonth.AddMonths(-1);
            int daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            for (int i = daysInPreviousMonth - offset + 1; i <= daysInPreviousMonth; i++)
            {
                Days.Add(i.ToString());
            }

            // Add days of the current month
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                Days.Add(i.ToString());
            }

            // Add days from the next month
            int nextDaysToAdd = 42 - Days.Count; // 42 is the total number of cells in a 6x7 grid
            for (int i = 1; i <= nextDaysToAdd; i++)
            {
                Days.Add(i.ToString());
            }
        }

        public void NextMonth()
        {
            currentDate = currentDate.AddMonths(1);
            FillDays(currentDate.Year, currentDate.Month);
        }

        private void PreviousMonth()
        {
            currentDate = currentDate.AddMonths(-1);
            FillDays(currentDate.Year, currentDate.Month);
        }

        public event PropertyChangedEventHandler? PropertyChanged; // Declare PropertyChanged as nullable

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Check if PropertyChanged is not null before invoking
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            NextMonth();
            OnPropertyChanged(nameof(Days));
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            PreviousMonth();
            OnPropertyChanged(nameof(Days));
        }
    }
}