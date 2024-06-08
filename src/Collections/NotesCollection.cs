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
        NotesList.Add(note);
        NoteKeys[note] = key;
    }

    public void Remove(Note note)
    {
        NotesList.Remove(note);
        NoteKeys.Remove(note);
    }

    public async void Edit(Note note, string title, string text, FirebaseClient firebaseClient)
    {
        if (NoteKeys.ContainsKey(note))
        {
            note.Title = title;
            note.Text = text;

            string key = NoteKeys[note];
            await firebaseClient
                .Child("Notes")
                .Child(key)
                .PutAsync(note);
        }
        else
        {
            throw new Exception("The note does not exist in the collection");
        }
    }

    public async void Draw(StackPanel noteButtonPanel, NotesView notesView, FirebaseClient firebaseClient)
    {
        noteButtonPanel.Children.Clear();
        
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
        using (StreamWriter stream = new StreamWriter(filename))
        {
            stream.WriteLine(note.Title);
            stream.WriteLine();
            stream.WriteLine(note.Text);
        }
    }
}