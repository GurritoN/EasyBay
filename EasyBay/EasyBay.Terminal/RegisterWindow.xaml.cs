using EasyBay.Terminal.API;
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
using System.Windows.Shapes;

namespace EasyBay.Terminal
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Login.Text))
            {
                Error.Text = "Login is invalid";
                return;
            }
            if (string.IsNullOrEmpty(Email.Text))
            {
                Error.Text = "Email is invalid";
                return;
            }
            if (string.IsNullOrEmpty(Password.Password))
            {
                Error.Text = "Password is invalid";
                return;
            }
            if (Password.Password == Confirm.Password)
            {
                if (await EasyBayClient.CreateUser(Login.Text, Password.Password, Email.Text))
                {
                    (Owner as MainWindow).Token = await EasyBayClient.GetToken(Login.Text, Password.Password);
                    (Owner as MainWindow).username = Login.Text;
                    Close();
                }
                else
                    Error.Text = "User already exists";
            }
            else
                Error.Text = "Passwords do not match";
        }
    }
}
