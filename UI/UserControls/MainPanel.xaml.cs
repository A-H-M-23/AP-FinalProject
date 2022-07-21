using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TransferFile;

namespace UI.UserControls
{
    public partial class MainPanel : UserControl
    {
        private bool LightTheme = true;
        public MainPanel()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            if (!Directory.Exists("Downloads"))
            {
                Directory.CreateDirectory("Downloads");
            }
            try
            {
                Listen.Start(int.Parse("8070"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exitbtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
            Listen.Stop();
        }

        public void AddMessageToUi(object o)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
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
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            int port = int.Parse("8070");

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
                        fileSend.Address = System.AppDomain.CurrentDomain.BaseDirectory + @"Downloads\" + Temp[Temp.Length - 1];
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
