using System.Windows;
using System.Windows.Navigation;

namespace Kalender_Project_FlorianRohat
{
    public static class Navigator
    {
        private static NavigationService NavigationService { get; } = ((MainWindow)Application.Current.MainWindow).MainFrame.NavigationService;

        public static void Navigate(string path, object param = null)
        {
            NavigationService.Navigate(new Uri(path, UriKind.RelativeOrAbsolute), param);
        }

        public static void GoBack()
        {
            NavigationService.GoBack();
        }

        public static void GoForward()
        {
            NavigationService.GoForward();
        }
    }
}