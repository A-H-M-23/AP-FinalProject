using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.UserControls
{
    public partial class Messagechat : UserControl
    {
        public Messagechat()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(Messagechat));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Messagechat));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
    }
}
