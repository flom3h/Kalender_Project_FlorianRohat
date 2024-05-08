using System.Windows;
using System.Windows.Controls;

namespace Kalender_Project_FlorianRohat;

public partial class NotesView : Page
{
    public NotesView()
    {
        InitializeComponent();
    }

    private void DisplayHome(object sender, RoutedEventArgs e)
    {
        if (this.NavigationService.CanGoBack)
        {
            this.NavigationService.GoBack();
        }
    }
}