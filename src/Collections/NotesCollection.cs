using System.IO;
using System.Windows.Controls;
using Firebase.Database;
using Firebase.Database.Query;

namespace Kalender_Project_FlorianRohat;

public class NotesCollection
{
    public List<Note> NotesList { get; set; } = new List<Note>();
    public Dictionary<Note, string> NoteKeys = new Dictionary<Note, string>();

    public void Add(Note note, string key)
    {
        Log.log.Information("NotesCollection: Add function called, adding note to list");
        NotesList.Add(note);
        NoteKeys[note] = key;
    }

    public void Remove(Note note)
    {
        Log.log.Information("NotesCollection: Remove function called, removing note from list");
        NotesList.Remove(note);
        NoteKeys.Remove(note);
    }

    public async void Edit(Note note, string title, string text, FirebaseClient firebaseClient)
    {
        Log.log.Information("NotesCollection: Edit function called, editing note in list");
        if (NoteKeys.ContainsKey(note))
        {
            note.Title = title;
            note.Text = text;

            string key = NoteKeys[note];
            Log.log.Information("NotesCollection: Updating Firebase database after edit");
            await firebaseClient
                .Child("Notes")
                .Child(key)
                .PutAsync(note);
        }
        else
        {
            Log.log.Warning("NotesCollection: Note does not exist in list");
            throw new Exception("The note does not exist in the collection");
        }
    }

    public async void Draw(StackPanel noteButtonPanel, NotesView notesView, FirebaseClient firebaseClient)
    {
        Log.log.Information("NotesCollection: Draw function called, drawing notes to stackpanel");
        noteButtonPanel.Children.Clear();
        
        Log.log.Information("NotesCollection: Pulling notes from Firebase database");
        var notes = await firebaseClient
            .Child("Notes")
            .OnceAsync<Note>();
        
        foreach (Note note in NotesList)
        {
            string key = NoteKeys[note];
            NoteItemControl noteItemControl = new NoteItemControl();
            noteItemControl.NoteTitle.Text = note.Title;
            noteItemControl.NoteButton.Click += (s, e) =>
            {
                notesView.NoteTextBox.IsEnabled = true;
                notesView.openedNote = note;
                notesView.NoteTextBox.Text = note.Text;
            };
            noteButtonPanel.Children.Add(noteItemControl);
        }
    }

    public void Export(Note note, string filename)
    {
        Log.log.Information("NotesCollection: Export function called, writing note to .txt file");
        using (StreamWriter stream = new StreamWriter(filename))
        {
            stream.WriteLine(note.Title);
            stream.WriteLine();
            stream.WriteLine(note.Text);
        }
    }
}