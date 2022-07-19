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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        public static int UserID = -1;
        public static string NameOfUser;
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This Method Is To Got Focus On The Username In The Login Form 
        /// Of The User Who Has Already Created The Account .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "Username")
                txtUserName.Text = "";
        }

        /// <summary>
        /// This Method Is To Lost Focus On The Username In The Login Form 
        /// Of The User Who Has Already Created The Account .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "")
                txtUserName.Text = "Username";
        }

        /// <summary>
        /// This Method Is To Got Focus On The Password In The Login Form
        /// Of The User Who Has Already Created The Account .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == "Password")
                txtPassword.Text = "";
        }

        /// <summary>
        /// This Method Is To Lost Focus On The Password In The Login Form
        /// Of The User Who Has Already Created The Account .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == "")
                txtPassword.Text = "Password";
        }

        private bool NullInputs()
        {
            if (txtUserName.Text != "Username")
            {
                if (txtPassword.Text != "Password")
                    return true;
                else
                {
                    MessageBox.Show("Please Enter Your Password");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please Enter Your Username");
                return false;
            }
        }

        /// <summary>
        /// This Method Is For Validating The Existence Or Non-Existence Of An Account .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (NullInputs())
            {
                if (CustomerRepository.customersList.Exists(user => user.UserName == txtUserName.Text))
                {
                    User temp = CustomerRepository.customersList.Where(user => user.UserName == txtUserName.Text).FirstOrDefault();
                    if (temp.HashPassword == PasswordSecurity.HashPassword(txtPassword.Text))
                    {
                        MessageBox.Show($"Welcome {temp.FirstName + " " + temp.LastName}");
                        UserID = temp.ID;
                        NameOfUser = temp.FirstName + " " + temp.LastName;
                        DialogResult = true;
                    }
                    else
                        MessageBox.Show("Wrong Password! Please Check Your Password");
                }
                else if (AdminRepository.adminsList.Exists(admin => admin.UserName == txtUserName.Text))
                {
                    User temp = AdminRepository.adminsList.Where(admin => admin.UserName == txtUserName.Text).FirstOrDefault();
                    if (temp.HashPassword == PasswordSecurity.HashPassword(txtPassword.Text))
                    {
                        MessageBox.Show($"Welcome {temp.FirstName + " " + temp.LastName}");
                        UserID = temp.ID;
                        NameOfUser = temp.FirstName + " " + temp.LastName;
                        DialogResult = true;
                    }
                    else
                        MessageBox.Show("Wrong Password! Please Check Your Password");
                }
                else
                    MessageBox.Show("This Username is not Exist");
            }
        }

        /// <summary>
        /// This Method Is For The User Who Wants To Create An Account
        /// And Has Filled In All The Fields Correctly .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow newAccountWindow = new NewAccountWindow();
            newAccountWindow.Owner = this;
            newAccountWindow.ShowDialog();
            if (newAccountWindow.DialogResult == true)
            {
                MessageBox.Show("Succesful Login :)");
                ReadJSON.User();
            }
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text != "Username" && txtUserName.Text != "")
                txtUserName.SetCurrentValue(ForegroundProperty, Brushes.Purple);
            else
                txtUserName.SetCurrentValue(ForegroundProperty, Brushes.Gray);
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text != "Password" && txtPassword.Text != "")
                txtPassword.SetCurrentValue(ForegroundProperty, Brushes.Purple);
            else
                txtPassword.SetCurrentValue(ForegroundProperty, Brushes.Gray);
        }
    }
}
