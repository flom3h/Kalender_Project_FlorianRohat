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
    }
    
    public AddNote (Note noteToEdit)
    {
        InitializeComponent();
        note = noteToEdit;
        NoteTitleInput.Text = note.Title;
    }
    
    private void CreateNote(object sender, RoutedEventArgs e)
    {
        Log.log.Information("AddNoteWindow: CreateNote() called, creating new note object");
        try
        {
            Log.log.Information("AddNoteWindow: Check if NoteTitleInput is empty");
            if (string.IsNullOrEmpty(NoteTitleInput.Text) == false)
            {
                note.Title = NoteTitleInput.Text;
                DialogResult = true;
            }
            else
            {
                Log.log.Warning("AddNoteWindow: NoteTitleInput is empty");
                MessageBox.Show("Gib etwas in die Felder ein", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception exception)
        {
            Log.log.Error(exception, "AddNoteWindow: Error while creating note");
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}