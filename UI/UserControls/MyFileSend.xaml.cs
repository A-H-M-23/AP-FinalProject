using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for MyFileSend.xaml
    /// </summary>
    public partial class MyFileSend : UserControl
    {
        public MyFileSend()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(MyFileSend));

        //Get & Set the name
        public string Username 
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty = DependencyProperty.Register("Address", typeof(string), typeof(MyFileSend));

        // Get & Set the Address of File
        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        //Open File With Click
        private void AttachFile_Click(object sender, RoutedEventArgs e) 
        {
            using (FileStream fs = File.OpenRead(Address))
            {
                Process.Start(Address);
            }
        }
    }
}
