using Microsoft.Win32;
using NAudio.Wave;
using System;
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
        private int IsAudioRecorded = 0;
        WaveIn wave;
        WaveFileWriter writer;

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

        private void FileSend(OpenFileDialog dialog)
        {
            IPAddress ipAddr = IPAddress.Parse(MainWindow.AddressIP);
            int port = int.Parse(MainWindow.Port);

            Dispatcher.Invoke(() =>
            {
                foreach (string fileName in dialog.FileNames)
                {
                    new Thread(() => { Sender.Send(ipAddr, port, fileName); }).Start();
                    MyFileSend fileSend = new MyFileSend();
                    string[] Temp = fileName.Split(@"\");
                    fileSend.Username = Temp[Temp.Length - 1];
                    fileSend.Address = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Downloads", Temp[Temp.Length - 1]);
                    AddMessageToUi(fileSend);
                }
            });
        }

        private void btnFileSelect_Click(object sender, RoutedEventArgs e)
        {
            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Files";

            if (file.ShowDialog() == true)
            {
                FileSend(file);
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
                Background.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
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
            SignUpWindow newAccountWindow = new SignUpWindow();
            SignUpWindow.Operation = 1;
            SignUpWindow.USERID = MainWindow.UserID;
            newAccountWindow.ShowDialog();
        }

        private void DisableAllButtons()
        {
            btnHome.IsActive = false;
            btnFiles.IsActive = false;
            btnVideo.IsActive = false;
            btnMusic.IsActive = false;
            btnImage.IsActive = false;
        }

        private void btnHome_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisableAllButtons();
            btnHome.IsActive = true;
            Condition.Text = "MR.Clone App";
        }

        private void btnFiles_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisableAllButtons();
            btnFiles.IsActive = true;
            Condition.Text = "MR.Clone App       Files";

            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Files";
            file.Filter = "Text files (*.txt)|*.txt|Rar files (*.rar)|*.rar|Zip files (*.zip)|*.zip|PDF files (*.pdf)|*.pdf";

            if (file.ShowDialog() == true)
            {
                FileSend(file);
            }
        }

        private void btnVideo_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisableAllButtons();
            btnVideo.IsActive = true;
            Condition.Text = "MR.Clone App       Video";

            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Images";
            file.Filter = "Mp4 files (*.mp4)|*.mp4|MOV files (*.mov)|*.mov|Mkv files (*.mkv)|*.mkv|Mpg files (*.mpg)|*.mpg|Avi files (*.avi)|*.avi";

            if (file.ShowDialog() == true)
            {
                FileSend(file);
            }
        }

        private void btnMusic_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisableAllButtons();
            btnMusic.IsActive = true;
            Condition.Text = "MR.Clone App       Music";

            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Images";
            file.Filter = "Mp3 files (*.mp3)|*.mp3|AAC files (*.aac)|*.aac|Ogg files (*.ogg)|*.ogg|Wav files (*.wav)|*.wav|Mp2 files (*.mp2)|*.mp2";

            if (file.ShowDialog() == true)
            {
                FileSend(file);
            }
        }

        private void btnImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisableAllButtons();
            btnImage.IsActive = true;
            Condition.Text = "MR.Clone App       Image";

            var file = new OpenFileDialog();
            file.Multiselect = true;
            file.Title = "Select Images";
            file.Filter = "Jpeg files (*.jpg)|*.jpg|Png files (*.png)|*.png|Tif files (*.tif)|*.tif|Gif files (*.gif)|*.gif|Svg files (*.svg)|*.svg|Bitmap files (*.bitmap)|*.bitmap";

            if (file.ShowDialog() == true)
            {
                FileSend(file);
            }
        }
        string current; 

        private void btnMicrophone_Click(object sender, RoutedEventArgs e)
        {
            if (UI.Setting.MicrophoneDevice != -1)
            {
                if (IsAudioRecorded == 0)
                {
                    IsAudioRecorded = 1;
                    wave = new WaveIn();
                    wave.WaveFormat = new WaveFormat(44100, 1);
                    wave.DeviceNumber = UI.Setting.MicrophoneDevice;
                    wave.DataAvailable += Wave_DataAvailable;
                    wave.RecordingStopped += Wave_RecordingStopped;
                    var filename = Guid.NewGuid().ToString() + ".wav";
                    current = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + @"Voices\" + filename);
                    writer = new WaveFileWriter(current, wave.WaveFormat);
                    wave.StartRecording();
                }
                else if (IsAudioRecorded == 1)
                {
                    IsAudioRecorded = 2;
                    wave.StopRecording();
                }
                else
                {
                    IsAudioRecorded = 0;
                    IPAddress ipAddr = IPAddress.Parse(MainWindow.AddressIP);
                    int port = int.Parse(MainWindow.Port);
                    Dispatcher.Invoke(() =>
                    {
                        new Thread(() => { Sender.Send(ipAddr, port, current); }).Start();
                        MyFileSend fileSend = new MyFileSend();
                        string[] Temp = current.Split(@"\");
                        fileSend.Username = Temp[Temp.Length - 1];
                        fileSend.Address = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Voices", Temp[Temp.Length - 1]);
                        AddMessageToUi(fileSend);
                    });
                }
            }
            else
            {
                Setting setting = new Setting();
                setting.ShowDialog();
            }
        }

        private void Wave_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            writer.Dispose();
        }

        private void Wave_DataAvailable(object? sender, WaveInEventArgs e)
        {
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void Setting_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }
    }
}
