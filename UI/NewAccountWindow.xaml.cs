using Business;
using Business.Repositories;
using Business.Validations;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UI
{
    /// <summary>
    /// Interaction logic for NewAccountWindow.xaml
    /// </summary>
    public partial class NewAccountWindow : System.Windows.Window
    {
        public static int Operation = 0;
        public static int USERID = -1;
        User user;
        private static string imagepath = "/Resources/Avatar.png";
        CustomerRepository repository;
        public NewAccountWindow()
        {
            InitializeComponent();
            user = new User();
            repository = new CustomerRepository();
        }

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

        /// <summary>
        /// This Method Is To Have No Null Inputs Or All Fields Are Filled .
        /// </summary>
        /// <returns>If All Fields Are Filled Return True And Else Return False</returns>
        public bool NullInputs()

            //Sign Up Boxes
        {
            if (txtFirstName.Text != "")
            {
                if (txtLastName.Text != "")
                {
                    if (txtUserName.Text != "")
                    {

                        if (txtPassword.Text != "")
                        {
                            if (txtEmailAddress.Text != "")
                            {
                                if (txtPhoneNumber.Text != "")
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
                    if (Password.PasswordSecurity(txtPassword.Text))
                    {
                        if (Email.EmailCheck(txtEmailAddress.Text))
                        {
                            user.FirstName = txtFirstName.Text;
                            user.LastName = txtLastName.Text;
                            user.UserName = txtUserName.Text;
                            user.PhoneNumber = txtPhoneNumber.Text;
                            user.Email = txtEmailAddress.Text;
                            user.HashPassword = PasswordSecurity.HashPassword(txtPassword.Text);
                            user.Address = txtHomeAddress.Text;
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
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtUserName.Text = user.UserName;
                txtEmailAddress.Text = user.Email;
                txtPhoneNumber.Text = user.PhoneNumber;
                if (user.Gender == GenderType.Male)
                    btnMale.IsChecked = true;
                else
                    btnFemale.IsChecked = true;
                txtHomeAddress.Text = user.Address;
                var convertor = new ImageSourceConverter();
                ProfilePhoto.Source = (ImageSource)convertor.ConvertFromString(user.ProfilePhoto);
            }
            if (Operation == 1)
            {
                TopLabel.Text = "Account Info";
                PasswordTab.Visibility = Visibility.Hidden;
                btnProfilePhoto.Visibility = Visibility.Hidden;
                txtFirstName.IsReadOnly = true;
                txtLastName.IsReadOnly = true;
                txtUserName.IsReadOnly = true;
                txtEmailAddress.IsReadOnly = true;
                txtPhoneNumber.IsReadOnly = true;
                btnMale.IsEnabled = false;
                btnFemale.IsEnabled = false;
                txtHomeAddress.IsReadOnly = true;
            }
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

        //Edit Account
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow accountWindow = new NewAccountWindow();
            NewAccountWindow.USERID = user.ID;
            NewAccountWindow.Operation = 2;
            this.Close();
            accountWindow.ShowDialog();
        }
        
        //Log Out
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
            mainWindow.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
