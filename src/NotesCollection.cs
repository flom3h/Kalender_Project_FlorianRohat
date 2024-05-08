using System.IO;
using System.Text.Json;

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