using System.Windows;

namespace Kalender_Project_FlorianRohat;

public partial class AddNote : Window
{
    public Note note { get; set; }
    public AddNote()
    {
        InitializeComponent();
        note = new Note();
        NoteTitleInput.Text = note.Title;
        NoteContentInput.Text = note.Text;
    }
    
    public AddNote (Note noteToEdit)
    {
        InitializeComponent();
        note = noteToEdit;
        NoteTitleInput.Text = note.Title;
        NoteContentInput.Text = note.Text;
    }
    
    private void CreateNote(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NoteTitleInput.Text) == false && string.IsNullOrEmpty(NoteContentInput.Text) == false)
        {
            note.Title = NoteTitleInput.Text;
            note.Text = NoteContentInput.Text;
            DialogResult = true;
        }
        else
        {
            MessageBox.Show("Gib etwas in die Felder ein", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}