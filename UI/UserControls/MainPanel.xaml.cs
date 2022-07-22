using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TransferFile;

namespace UI.UserControls
{
    public partial class MainPanel : UserControl
    {
        private bool LightTheme = true;
        public MainPanel()
        {
            InitializeComponent();
        }

        private void Exitbtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
            Listen.Stop();
        }

        public void AddMessageToUi(object o)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (Application.Current != null)
                dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(() =>
                {
                    Container.Children.Add((UIElement)o);
                    ContainerScroll.ScrollToEnd();
                });
                return;
            }
            else
            {
                Container.Children.Add((UIElement)o);
                ContainerScroll.ScrollToEnd();
            }
        }

        private void btnFileSelect_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ipAddr = IPAddress.Parse(MainWindow.AddressIP);
            int port = int.Parse(MainWindow.Port);

            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Files";

            if (file.ShowDialog() == true)
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (string fileName in file.FileNames)
                    {
                        new Thread(() => { Sender.Send(ipAddr, port, fileName); }).Start();
                        MyFileSend fileSend = new MyFileSend();
                        string[] Temp = fileName.Split(@"\");
                        fileSend.Username = Temp[Temp.Length - 1];
                        fileSend.Address = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory , "Downloads" , Temp[Temp.Length - 1]);
                        AddMessageToUi(fileSend);
                    }
                });
            }
        }

        private void LogOut_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            Listen.Stop();
            Process.Start(currentExecutablePath);
            Application.Current.Shutdown();
        }

        private void btnTheme_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (LightTheme)
            {
                Background.Background = new SolidColorBrush(Color.FromRgb(48 , 48 , 48));
                ContainerScroll.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
                Page.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
                LightTheme = false;
            }
            else
            {
                LightTheme = true;
                Background.Background = new SolidColorBrush(Color.FromRgb(248, 247, 252));
                ContainerScroll.Background = new SolidColorBrush(SystemColors.WindowColor);
                Page.Background = new SolidColorBrush(SystemColors.WindowColor);
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow newAccountWindow = new NewAccountWindow();
            NewAccountWindow.Operation = 1;
            NewAccountWindow.USERID = MainWindow.UserID;
            newAccountWindow.ShowDialog();
        }
    }
}
