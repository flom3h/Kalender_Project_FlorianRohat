using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Kalender_Project_FlorianRohat;

public partial class NotesView : Page
{
    public static NotesCollection notesCollection;
    public Note openedNote;
    
    public NotesView()
    {
        InitializeComponent();
        notesCollection = new NotesCollection();
        DisplayTime();
    }

    private async void AddNote(object sender, RoutedEventArgs e)
    {
        AddNote addNote = new AddNote();
        if (addNote.ShowDialog() == true)
        {
            notesCollection.Add(addNote.note);
            notesCollection.Draw(NoteButtonPanel, this);
        }
    }

    private void DeleteNote(object sender, RoutedEventArgs e)
    {
        if (openedNote != null)
        {
            notesCollection.Remove(openedNote);
            notesCollection.Draw(NoteButtonPanel, this);
            NoteTextBox.Text = string.Empty;
            NoteTextBox.IsEnabled = false;
            openedNote = null;
        }
        else
        {
            MessageBox.Show("No note is currently opened", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private async void SaveNote(object sender, RoutedEventArgs e)
    {
        if (openedNote != null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Note";
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                notesCollection.Export(openedNote, saveFileDialog.FileName);
            }
        }
        else
        {
            MessageBox.Show("Du hast keine Notiz geöffnet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        if (this.NavigationService.CanGoBack)
        {
            this.NavigationService.GoBack();
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

    private void NoteTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (openedNote != null)
        {
            notesCollection.Edit(openedNote, openedNote.Title, NoteTextBox.Text);
        }
    }
}