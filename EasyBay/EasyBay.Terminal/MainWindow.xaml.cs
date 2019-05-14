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
            Balance.Text = user.FreeBalance.ToString() + " USD";
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
                Border border = new Border();
                border.BorderThickness = new Thickness(0, 0, 0, 1);
                border.BorderBrush = Brushes.Black;

                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"D:\EasyBay\EasyBay\EasyBay.Terminal\img\default.jpg", UriKind.Absolute));
                image.Width = 100;
                panel.Children.Add(image);

                TextBlock name = new TextBlock();
                name.FontFamily = new FontFamily("Georgia");
                name.Margin = new Thickness(10, 0, 0, 0);
                name.Text = lot.Name;
                name.Width = 490;
                name.FontSize = 50;
                panel.Children.Add(name);

                Button details = new Button();
                details.Content = "Details";
                details.HorizontalAlignment = HorizontalAlignment.Center;
                details.VerticalAlignment = VerticalAlignment.Center;
                details.Height = 30;
                details.Width = 100;
                details.Click += (s, e) =>
                {
                    LotDetailsWindow lWindow = new LotDetailsWindow(client, lot.Id, user.Id);
                    lWindow.Owner = this;
                    lWindow.ShowDialog();
                    Refresh();
                };
                panel.Children.Add(details);

                border.Child = panel;
                LotList.Children.Add(border);
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

        private void RightPage(object sender, RoutedEventArgs e)
        {
            page++;
            GetPage();
        }

        private void LeftPage(object sender, RoutedEventArgs e)
        {
            page--;
            GetPage();
        }

        private void UserPage(object sender, RoutedEventArgs e)
        {
            UserWindow uWindow = new UserWindow(client, user);
            uWindow.Owner = this;
            uWindow.ShowDialog();
            Refresh();
        }
    }
}
