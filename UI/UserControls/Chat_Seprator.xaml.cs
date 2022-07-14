using System.Windows;
using System.Windows.Controls;

namespace UI.UserControls
{
    public partial class Chat_Seprator : UserControl
    {
        public Chat_Seprator()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Chat_Seprator));

        public string Title 
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
