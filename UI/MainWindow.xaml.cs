﻿using Business;
using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TransferFile;
using UI.UserControls;

namespace UI
{
    public partial class MainWindow : Window
    {
        WelcomePanel welcome;
        MainPanel main;
        Login login;
        public static bool IsClient = false;

        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Port { get; set; }
        public static string AddressIP { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ReadJSON.User();
            welcome = new WelcomePanel();
            main = new MainPanel();
            login = new Login();
            MainPage.Children.Add(main);
            MainPage.Children.Add(welcome);
            MainPage.Children.Add(login);
        }

        public static void init()
        {
            if (!Directory.Exists("Downloads"))
            {
                Directory.CreateDirectory("Downloads");
            }
            if(!IsClient)
            {
                try
                {

                    Listen.Start(int.Parse(Port));

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ( e.ChangedButton == MouseButton.Left )
            {
                this.DragMove();
            }
        }

        bool IsMaximized = false;

        //tuning the window
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if(IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1250;
                    this.Height = 830;
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }

        //Close Buttons
        private void Exitbtn_Click (object sender, RoutedEventArgs e)
        {
            this.Close();
            Listen.Stop();
        }

        //Function that send Ip & port . We can have Multiselect in App
        private void btnFileSelect_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            int port = int.Parse("8070");

            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Files";

            if(file.ShowDialog() == true)
            {
                foreach (string fileName in file.FileNames)
                {
                    new Thread(() => { Sender.Send(ipAddr, port, fileName); }).Start();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
            Listen.Stop();
        }

    }
}
