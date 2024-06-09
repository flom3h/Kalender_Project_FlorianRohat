using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Win32;

namespace Kalender_Project_FlorianRohat;

public partial class NotesView : Page
{
    public static NotesCollection notesCollection;
    public static event Action DisplayAllTodosEvent;
    private FirebaseClient firebaseClient;
    public Note? openedNote;
    public static event EventHandler NotesButtonClicked;
    public static event EventHandler CalendarButtonClicked;
    public static event EventHandler EventButtonClicked; 
    public static event EventHandler AllTodosButtonClicked;
    public static event EventHandler ImportantTodosButtonClicked;
    public static event EventHandler HomeButtonClicked; 
    
    public NotesView()
    {
        Log.log.Information("NotesView opened");
        InitializeComponent();
        notesCollection = new NotesCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        LoadNotes();
        Log.log.Information("NotesView initialized, components loaded");
    }

    private async void AddNote(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: AddNote function called, opening AddNoteWindow");
        AddNote addNote = new AddNote();
        Log.log.Information("NotesView: Check if AddNoteWindow is true");
        if (addNote.ShowDialog() == true)
        {
            Log.log.Information("NotesView: AddNoteWindow is true, adding note to firebase database");
            var addedNote = await firebaseClient
                .Child("Notes")
                .PostAsync(addNote.note);
            Log.log.Information("NotesView: added note to firebase database");
            notesCollection.Add(addNote.note, addedNote.Key);
            Log.log.Information("NotesView: added note to collection");
            notesCollection.Draw(NoteButtonPanel, this, firebaseClient);
        }
    }

    private async void DeleteNote(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: DeleteNote function called, deleting note from firebase database");
        Log.log.Information("NotesView: Check if note is opened");
        if (openedNote != null)
        {
            string key = notesCollection.NoteKeys[openedNote];
            await firebaseClient
                .Child("Notes")
                .Child(key)
                .DeleteAsync();
            Log.log.Information("NotesView: removed note from firebase database");
            notesCollection.Remove(openedNote);
            Log.log.Information("NotesView: removed note from collection");
            notesCollection.Draw(NoteButtonPanel, this, firebaseClient);
            Log.log.Information("NotesView: redrawing notes");
            openedNote = null;
            NoteTextBox.Text = string.Empty;
            NoteTextBox.IsEnabled = false;
        }
        else
        {
            Log.log.Warning("NoteView: No note is currently opened");
            MessageBox.Show("No note is currently opened", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        Log.log.Information("NotesView: Checking if note is opened");
        if (openedNote == null)
        {
            WatermarkText.Text = "Klick auf eine Notiz um diese zu Öffnen";
        }
    }
    
    private void SaveNote(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: SaveNote function called, opening SaveFileDialog");
        Log.log.Information("NotesView: Check if note is opened");
        if (openedNote != null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Note";
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Log.log.Information("NotesView: Check if SaveFileDialog is true");
            if (saveFileDialog.ShowDialog() == true)
            {
                Log.log.Information("NotesView: SaveFileDialog is true, writing note to text file");
                notesCollection.Export(openedNote, saveFileDialog.FileName);
            }
        }
        else
        {
            Log.log.Warning("NotesView: No note is currently opened");
            MessageBox.Show("Du hast keine Notiz geöffnet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: Home button clicked, changing to MainPage");
        HomeButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    private void DisplayNotes(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: Home button clicked, changing to NotesView");
        NotesButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayCalendar(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: Calendar button clicked, changing to CalendarView");
        CalendarButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void DisplayEvents(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: Event button clicked, changing to EventView");
        EventButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: AllTodos button clicked, changing to AllTodosView");
        AllTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayImportantTodos(object sender, RoutedEventArgs e)
    {
        Log.log.Information("NotesView: ImportantTodos button clicked, changing to ImportantView");
        ImportantTodosButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    
    private void DisplayTime()
    {
        Log.log.Information("NotesView: DisplayTime function called, displaying current time");
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
        Log.log.Information("NotesView: NoteTextBox_OnTextChanged function called, saving note");
        try
        {
            Log.log.Information("NotesView: Checking if note Title is opened note Title");
            foreach (NoteItemControl control in NoteButtonPanel.Children)
            {
                if (control.NoteTitle.Text == openedNote?.Title)
                {
                    control.NoteTitle.FontWeight = FontWeights.Bold;
                }
                else
                {
                    control.NoteTitle.FontWeight = FontWeights.Normal;
                }
            }

            Log.log.Information("NotesView: Checking if note is opened");
            if (openedNote != null)
            {
                notesCollection.Edit(openedNote, openedNote.Title, NoteTextBox.Text, firebaseClient);
            }
        }
        catch (Exception exception)
        {
            Log.log.Error(exception, "NotesView: Error while saving note");
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private async void LoadNotes()
    {
        Log.log.Information("NotesView: LoadNotes function called, loading notes from firebase database");
        var notes = await firebaseClient
            .Child("Notes")
            .OnceAsync<Note>();
        Log.log.Information("NotesView: Loaded notes from firebase database");

        Log.log.Information("NotesView: Adding notes to collection");
        foreach (var note in notes)
        {
            notesCollection.Add(note.Object, note.Key);
        }
        notesCollection.Draw(NoteButtonPanel, this, firebaseClient);
        Log.log.Information("NotesView: Check if note is opened");
        if (openedNote == null)
        {
            WatermarkText.Text = "Klick auf eine Notiz um diese zu Öffnen";
        }
    }
}