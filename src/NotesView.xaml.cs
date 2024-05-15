using System.Windows;
using System.Windows.Controls;

namespace Kalender_Project_FlorianRohat;

public partial class NotesView : Page
{
    public static NotesCollection notesCollection;
    
    public NotesView()
    {
        InitializeComponent();
        notesCollection = new NotesCollection();
    }

    private async void AddNote(object sender, RoutedEventArgs e)
    {
        AddNote addNote = new AddNote();
        if (addNote.ShowDialog() == true)
        {
            notesCollection.Add(addNote.note);
            notesCollection.Draw(NoteButtonPanel);
        }
    }

    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        if (this.NavigationService.CanGoBack)
        {
            this.NavigationService.GoBack();
        }
    }
}