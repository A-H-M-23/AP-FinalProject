using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.UserControls
{
    public partial class UserImage : UserControl
    {
        public UserImage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(UserImage));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
