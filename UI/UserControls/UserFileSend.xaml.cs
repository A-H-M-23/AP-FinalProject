using System.Windows;
using System.Windows.Controls;

namespace UI.UserControls
{
    public partial class UserFileSend : UserControl
    {
        public UserFileSend()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(UserChat));

        //User Name
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty = DependencyProperty.Register("Address", typeof(string), typeof(UserFileSend));

        //Address of File
        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
