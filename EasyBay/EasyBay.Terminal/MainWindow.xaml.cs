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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyBay.Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EasyBayClient client;
        private User user;
        public string username;
        public string Token;

        private int page;

        public MainWindow()
        {
            InitializeComponent();
            this.ContentRendered += OnContentRendered; ;
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            LoginWindow lWindow = new LoginWindow();
            lWindow.Owner = this;
            lWindow.ShowDialog();
            if (!string.IsNullOrEmpty(Token))
                OnLogin();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            RegisterWindow rWindow = new RegisterWindow();
            rWindow.Owner = this;
            rWindow.ShowDialog();
            OnLogin();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            LoginWindow lWindow = new LoginWindow();
            lWindow.Owner = this;
            lWindow.ShowDialog();
            if (!string.IsNullOrEmpty(Token))
                OnLogin();
        }

        private async void OnLogin()
        {
            client = new EasyBayClient(Token);
            user = await client.GetUser(username);
            Username.Text = username;
            Balance.Text = user.Balance.ToString() + " USD";
            page = 0;
            Refresh();
        }

        private async void GetPage()
        {
            List<Lot> lots = await client.GetLotPage(page, 10);
            if (lots.Count == 0)
                return;
            LotList.Children.Clear();
            foreach (var lot in lots)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                TextBlock name = new TextBlock();
                name.Text = lot.Name;
                name.Width = 100;
                name.FontSize = 50;
                panel.Children.Add(name);

                Button details = new Button();
                details.Content = "Details";
                details.Click += (s, e) =>
                {
                    MessageBox.Show($"{lot.Id}");
                };
                panel.Children.Add(details);

                LotList.Children.Add(panel);
            }
        }

        private void Refresh()
        {
            page = 0;
            GetPage();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CreateLot(object sender, RoutedEventArgs e)
        {
            CreateLotWindow clWindow = new CreateLotWindow(client);
            clWindow.Owner = this;
            clWindow.ShowDialog();
            Refresh();
        }
    }
}
