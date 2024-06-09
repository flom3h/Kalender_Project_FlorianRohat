using System.Windows;
using System.Windows.Navigation;

namespace Kalender_Project_FlorianRohat
{
    public static class Navigator
    {
        private static NavigationService NavigationService { get; } = ((MainWindow)Application.Current.MainWindow).MainFrame.NavigationService;

        public static void Navigate(string path, object param = null)
        {
            Log.log.Information($"Navigator: Navigating to Page: {path}");
            NavigationService.Navigate(new Uri(path, UriKind.RelativeOrAbsolute), param);
        }

        public static void GoBack()
        {
            Log.log.Information("Navigator: Returning to previous page");
            NavigationService.GoBack();
        }

        public static void GoForward()
        {
            Log.log.Information("Navigator: Going to next page");
            NavigationService.GoForward();
        }
    }
}