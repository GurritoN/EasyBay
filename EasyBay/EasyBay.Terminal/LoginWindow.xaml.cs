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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginBtn(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Login.Text))
            {
                Error.Text = "Login is invalid";
                return;
            }
            if (string.IsNullOrEmpty(Password.Password))
            {
                Error.Text = "Password is invalid";
                return;
            }
            string token = await EasyBayClient.GetToken(Login.Text, Password.Password);
            if (string.IsNullOrEmpty(token))
            {
                Error.Text = "Invalid credentials";
                return;
            }
            (Owner as MainWindow).Token = token;
            (Owner as MainWindow).username = Login.Text;
            Close();
        }
    }
}
