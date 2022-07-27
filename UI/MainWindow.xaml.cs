using Business;
using System;
using System.IO;
using System.Windows;
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
            if (!File.Exists("./UserData.txt"))
            {
                File.Create("./UserData.txt");
            }
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
            if (!IsClient)
            {
                try
                {
                    Listen.AddressIP = AddressIP;
                    Listen.Start(int.Parse(Port));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Listen.Stop();
        }
    }
}
