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
    
    public NotesView()
    {
        InitializeComponent();
        notesCollection = new NotesCollection();
        firebaseClient =  new FirebaseClient("https://kalenderprojectflorianro-default-rtdb.europe-west1.firebasedatabase.app/");
        DisplayTime();
        LoadNotes();
    }

    private async void AddNote(object sender, RoutedEventArgs e)
    {
        AddNote addNote = new AddNote();
        if (addNote.ShowDialog() == true)
        {
            var addedNote = await firebaseClient
                .Child("Notes")
                .PostAsync(addNote.note);
            notesCollection.Add(addNote.note, addedNote.Key);
            notesCollection.Draw(NoteButtonPanel, this, firebaseClient);
        }
    }

    private async void DeleteNote(object sender, RoutedEventArgs e)
    {
        if (openedNote != null)
        {
            string key = notesCollection.NoteKeys[openedNote];
            await firebaseClient
                .Child("Notes")
                .Child(key)
                .DeleteAsync();
            notesCollection.Remove(openedNote);
            notesCollection.Draw(NoteButtonPanel, this, firebaseClient);
            openedNote = null;
            NoteTextBox.Text = string.Empty;
            NoteTextBox.IsEnabled = false;
        }
        else
        {
            MessageBox.Show("No note is currently opened", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        if (openedNote == null)
        {
            WatermarkText.Text = "Klick auf eine Notiz um diese zu Öffnen";
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
    
    private void DisplayAllTodos(object sender, RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new MainPage());
        DisplayAllTodosEvent?.Invoke();
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

        if (openedNote != null)
        {
            notesCollection.Edit(openedNote, openedNote.Title, NoteTextBox.Text, firebaseClient);
        }
    }

    private async void LoadNotes()
    {
        var notes = await firebaseClient
            .Child("Notes")
            .OnceAsync<Note>();

        foreach (var note in notes)
        {
            notesCollection.Add(note.Object, note.Key);
        }
        notesCollection.Draw(NoteButtonPanel, this, firebaseClient);
        if (openedNote == null)
        {
            WatermarkText.Text = "Klick auf eine Notiz um diese zu Öffnen";
        }
    }
}