using System.Windows;
using System.Windows.Controls;

namespace UI.UserControls
{
    public partial class MyMessageChat : UserControl
    {
        public MyMessageChat()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MyMessageChat));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
    }
}
