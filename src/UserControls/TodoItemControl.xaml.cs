using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalender_Project_FlorianRohat
{
    /// <summary>
    /// Interaction logic for TodoItemControl.xaml
    /// </summary>
    public partial class TodoItemControl : UserControl
    {

        public TodoItemControl()
        {
            InitializeComponent();
            StarIcon = this.FindName("StarIcon") as MaterialDesignThemes.Wpf.PackIcon;
        }
    }
}
