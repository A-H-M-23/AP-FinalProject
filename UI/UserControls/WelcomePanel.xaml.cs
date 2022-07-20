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

namespace UI.UserControls
{
    // A user control that use for Welcome panel(Begining Panel)
    public partial class WelcomePanel : UserControl
    {
        public WelcomePanel()
        {
            InitializeComponent();
        }

        private void btnSend_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = true;
            //Remove
            (this.Parent as Grid).Children.Remove(this);
        }

        private void btnReceive_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = false;
            (this.Parent as Grid).Children.Remove(this);
        }

        //If double Click the btn Send Obj
        private void btnSend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = true;
            (this.Parent as Grid).Children.Remove(this);
        }

        private void btnReceive_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = false;
            (this.Parent as Grid).Children.Remove(this);
        }
    }
}
