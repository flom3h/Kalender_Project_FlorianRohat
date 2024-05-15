using System.IO;
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

    public void Draw(StackPanel NoteButtonPanel)
    {
        NoteButtonPanel.Children.Clear();
        foreach (Note note in NotesList)
        {
            NoteItemControl noteItemControl = new NoteItemControl();
            noteItemControl.NoteTitle.Text = note.Title;
            noteItemControl.NoteTextDemo.Text = note.Text.Length > 50 ? note.Text.Substring(0, 50) : note.Text;
            NoteButtonPanel.Children.Add(noteItemControl);
        }
    }
    
    public void Serialize(string filename)
    {
        using (StreamWriter stream = new StreamWriter(filename))
        {
            foreach (var note in NotesList)
            {
                string jsonStr = JsonSerializer.Serialize(note);
                stream.WriteLine(jsonStr);
            }
        }
    }
}