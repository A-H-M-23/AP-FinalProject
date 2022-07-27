using NAudio.Wave;
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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        public static int MicrophoneDevice { get; set; } = -1;
        public static int SpeakerDevices { get; set; } = -1;
        public Setting()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            for(int deviceID = 0; deviceID < WaveIn.DeviceCount; deviceID++)
            {
                var deviceinfo = WaveIn.GetCapabilities(deviceID);
                Microphone.Items.Add(deviceinfo.ProductName);
            }
            for (int deviceID = 0; deviceID < WaveOut.DeviceCount; deviceID++)
            {
                var deviceinfo = WaveOut.GetCapabilities(deviceID);
                Speaker.Items.Add(deviceinfo.ProductName);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MicrophoneDevice = Microphone.SelectedIndex;
            SpeakerDevices = Speaker.SelectedIndex;
            DialogResult = true;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
