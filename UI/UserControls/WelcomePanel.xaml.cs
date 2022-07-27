using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.UserControls
{
    // A user control that use for Welcome panel(Begining Panel)
    public partial class WelcomePanel : UserControl
    {
        public WelcomePanel()
        {
            InitializeComponent();
            SetIP();
        }

        public static Task<List<string>> GetAllIpAsync()
        {
            List<string> list = new List<string>();
            string myHost = System.Net.Dns.GetHostName();

            System.Net.IPHostEntry myIPs = System.Net.Dns.GetHostEntry(myHost);

            foreach (var item in myIPs.AddressList)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    list.Add(item.ToString());
            }

            return Task.FromResult(list);
        }

        async void SetIP()
        {
            var res = await GetAllIpAsync();
            foreach (var item in res)
            {
                addressIP.Items.Add(new ComboBoxItem() { Content = item });
            }
            addressIP.SelectedIndex = 1;
        }

        private void btnSend_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = true;
            if (!(port.Text == "" || addressIP.SelectedValue == ""))
            {
                MainWindow.Port = port.Text;
                string[] temp = addressIP.SelectedItem.ToString().Split(" ");
                MainWindow.AddressIP = temp[1];
                MainWindow.init();
                (this.Parent as Grid).Children.Remove(this);
            }
            else
                MessageBox.Show("Please Check Your Inputs !!!");
        }

        private void btnReceive_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = false;
            if (!(port.Text == "" || addressIP.SelectedValue == ""))
            {
                MainWindow.Port = port.Text;
                string[] temp = addressIP.SelectedItem.ToString().Split(" ");
                MainWindow.AddressIP = temp[1];
                MainWindow.init();
                (this.Parent as Grid).Children.Remove(this);
            }
            else
                MessageBox.Show("Please Check Your Inputs !!!");
        }

        //If double Click the btn Send Obj
        private void btnSend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = true;
            if (!(port.Text == "" || addressIP.SelectedValue == ""))
            {
                MainWindow.Port = port.Text;
                string[] temp = addressIP.SelectedItem.ToString().Split(" ");
                MainWindow.AddressIP = temp[1];
                MainWindow.init();
                (this.Parent as Grid).Children.Remove(this);
            }
            else
                MessageBox.Show("Please Check Your Inputs !!!");
        }

        private void btnReceive_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.IsClient = false;
            if (!(port.Text == "" || addressIP.SelectedValue == ""))
            {
                MainWindow.Port = port.Text;
                string[] temp  = addressIP.SelectedItem.ToString().Split(" ");
                MainWindow.AddressIP = temp[1];
                MainWindow.init();
                (this.Parent as Grid).Children.Remove(this);
            }
            else
                MessageBox.Show("Please Check Your Inputs !!!");
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
