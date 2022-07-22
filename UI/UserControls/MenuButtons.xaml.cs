using System.Windows;
using System.Windows.Controls;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for MenuButtons.xaml
    /// </summary>
    public partial class MenuButtons : UserControl
    {
        public MenuButtons()
        {
            InitializeComponent();
        }

        //Get & Set the title of buttons
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MenuButtons));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(MenuButtons));

        public MahApps.Metro.IconPacks.PackIconMaterialKind Icon
        {
            get { return (MahApps.Metro.IconPacks.PackIconMaterialKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(MahApps.Metro.IconPacks.PackIconMaterialKind), typeof(MenuButtons));
    }
}
