using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Windows.Controls;

namespace Kalender_Project_FlorianRohat;

public class NotesCollection
{
    public List<Note> NotesList { get; set; } = new List<Note>();
    private Dictionary<Note, string> NoteKeys = new Dictionary<Note, string>();

    public void Add(Note note)
    {
        NotesList.Add(note);
    }

    public void Remove(Note note)
    {
        NotesList.Remove(note);
    }

    public void Edit(Note note, string title, string text)
    {
        note.Title = title;
        note.Text = text;
    }

    public void Draw(StackPanel NoteButtonPanel, NotesView notesView)
    {
        NoteButtonPanel.Children.Clear();
        foreach (Note note in NotesList)
        {
            NoteItemControl noteItemControl = new NoteItemControl();
            noteItemControl.NoteTitle.Text = note.Title;
            noteItemControl.NoteButton.Click += (s, e) =>
            {
                notesView.NoteTextBox.IsEnabled = true;
                notesView.openedNote = note;
                notesView.NoteTextBox.Text = note.Text;
            };
            NoteButtonPanel.Children.Add(noteItemControl);
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