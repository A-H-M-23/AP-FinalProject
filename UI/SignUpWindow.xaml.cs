using Business;
using Business.Repositories;
using Business.Validations;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UI
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public static string MalePic = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "Male.png");
        public static string FemalePic = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "Female.png");
        public static int Operation = 0;
        public static int USERID = -1;
        User user;
        private static string imagepath = "/Resources/Avatar.png";
        CustomerRepository repository;
        public SignUpWindow()
        {
            InitializeComponent();
            user = new User();
            repository = new CustomerRepository();
        }

        #region Profile PictureSet
        /// <summary>
        /// This Method Is For Set Customers Profile Pictures .
        /// </summary>
        public void ProfilePictureSet()
        {
            string ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(ProfilePhoto.Source.ToString());
            string path = "./Images/Profiles/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string dispath = path + ImageName;
            user.ProfilePhoto = dispath;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)ProfilePhoto.Source));
            using (FileStream stream = new FileStream(dispath, FileMode.Create))
                encoder.Save(stream);
        }
        #endregion

        #region Null Inputs
        /// <summary>
        /// This Method Is To Have No Null Inputs Or All Fields Are Filled .
        /// </summary>
        /// <returns>If All Fields Are Filled Return True And Else Return False</returns>
        public bool NullInputs()
        {
            if (txtFirstname.Text != "")
            {
                if (txtLastname.Text != "")
                {
                    if (txtUsername.Text != "")
                    {
                        if (txtPassword.Password != "")
                        {
                            if (txtEmail.Text != "")
                            {
                                if (txtMobile.Text != "")
                                {
                                    if (btnMale.IsChecked == true || btnFemale.IsChecked == true)
                                        return true;
                                    else
                                    {
                                        MessageBox.Show("Please Select Your Gender");
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please Enter Your PhoneNumber");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter Your Email");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Enter Your Password");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Your UserName");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Your LastName");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please Enter Your Name");
                return false;
            }
        }
        #endregion

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnMale_Click(object sender, RoutedEventArgs e)
        {
            btnMale.Background = new SolidColorBrush(Color.FromRgb(97, 70, 227));
            btnFemale.Background = new SolidColorBrush(SystemColors.ActiveBorderBrush.Color);
            ProfilePhoto.Source = new BitmapImage(new Uri(MalePic));
        }

        private void btnFemale_Click(object sender, RoutedEventArgs e)
        {
            btnFemale.Background = new SolidColorBrush(Color.FromRgb(97, 70, 227));
            btnMale.Background = new SolidColorBrush(SystemColors.ActiveBorderBrush.Color);
            ProfilePhoto.Source = new BitmapImage(new Uri(FemalePic));
        }

        /// <summary>
        /// This Methd Is For Validation The Input Of Fields .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            ///Create New Account
            if (Operation != 1)
            {
                if (NullInputs())
                {
                    if (Password.PasswordSecurity(txtPassword.Password))
                    {
                        if (Email.EmailCheck(txtEmail.Text))
                        {
                            user.FirstName = txtFirstname.Text;
                            user.LastName = txtLastname.Text;
                            user.UserName = txtUsername.Text;
                            user.PhoneNumber = txtMobile.Text;
                            user.Email = txtEmail.Text;
                            user.HashPassword = PasswordSecurity.HashPassword(txtPassword.Password);
                            user.Gender = (btnMale.IsChecked == true) ? GenderType.Male : GenderType.Female;
                            ProfilePictureSet();
                            if (Operation == 0)
                            {
                                user.ID = CustomerRepository.customersList.Count();
                                repository.WriteJson(user);
                            }
                            if (Operation == 2)
                                repository.Update(user);
                            DialogResult = true;
                        }
                        else
                            MessageBox.Show("Invalid Email !! Please Check Your Email Address");
                    }
                    else
                        MessageBox.Show("Your Password is Unsafe\nPassword Should Be at least 8 Charachter and has small , capital letters and Symbols also Numbers");
                }
            }
            else
                DialogResult = false;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
            mainWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow accountWindow = new SignUpWindow();
            SignUpWindow.USERID = user.ID;
            SignUpWindow.Operation = 2;
            this.Close();
            accountWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"{user.FirstName} {user.LastName} Are You Sure Delete Your Account ?", "Delete Account", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                repository.Delete(user);
            MainWindow mainWindow = new MainWindow();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
            mainWindow.ShowDialog();
        }

        /// <summary>
        /// This Method Is For Selecting The User's Profile Picture .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProfilePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            if (Dialog.ShowDialog() == true)
            {
                imagepath = Dialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagepath);
                bitmap.EndInit();
                ProfilePhoto.Source = bitmap;
            }
        }

        /// <summary>
        /// This Method Is To Specify The Role Of The User (Admin Or Customer)
        /// And Validate The Accessibility for Each Role .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Operation != 1)
            {
                btnDelete.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
                btnLogOut.Visibility = Visibility.Hidden;
            }
            if (Operation == 2)
            {
                btnNewAccount.Content = "Edit";
                TopLabel.Text = "Edit Account";
            }
            if (Operation != 0)
            {
                user = CustomerRepository.customersList.FirstOrDefault(c => c.ID == USERID);
                btnNewAccount.Content = "Confirm";
                txtFirstname.Text = user.FirstName;
                txtLastname.Text = user.LastName;
                txtUsername.Text = user.UserName;
                txtEmail.Text = user.Email;
                txtMobile.Text = user.PhoneNumber;
                if (user.Gender == GenderType.Male)
                    btnMale.IsChecked = true;
                else
                    btnFemale.IsChecked = true;
                var convertor = new ImageSourceConverter();
                ProfilePhoto.Source = (ImageSource)convertor.ConvertFromString(user.ProfilePhoto);
            }
            if (Operation == 1)
            {
                TopLabel.Text = "Account Info";
                btnProfilePhoto.Visibility = Visibility.Hidden;
                txtFirstname.IsReadOnly = true;
                txtLastname.IsReadOnly = true;
                txtUsername.IsReadOnly = true;
                txtEmail.IsReadOnly = true;
                txtMobile.IsReadOnly = true;
                btnMale.IsEnabled = false;
                btnFemale.IsEnabled = false;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
