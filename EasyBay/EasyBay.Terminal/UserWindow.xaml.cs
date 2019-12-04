using EasyBay.Terminal.API;
using EasyBay.Terminal.Models;
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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        EasyBayClient client;
        User user;
        public UserWindow(EasyBayClient client, User user)
        {
            InitializeComponent();
            this.client = client;
            this.user = user;
            Username.Text = user.Username;
            Email.Text = user.Email;
            Balance.Text = user.Balance.ToString();
            Locked.Text = user.LockedBalance.ToString();
        }

        private async void Deposit(object sender, RoutedEventArgs e)
        {
            if (await client.Deposit(decimal.Parse(Amount.Text)))
                Balance.Text = (user.Balance + decimal.Parse(Amount.Text)).ToString();
        }
    }
}
