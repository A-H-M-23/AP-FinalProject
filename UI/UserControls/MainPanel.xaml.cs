using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TransferFile;

namespace UI.UserControls
{
    public partial class MainPanel : UserControl
    {
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

        private void AddMessageToUi(object o)
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
    }
}
