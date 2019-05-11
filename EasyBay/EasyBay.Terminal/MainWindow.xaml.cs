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
        private string token;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            EasyBayClient.CreateUser("GurritoN", "Apz3cmcm", "gurriton@yandex.ru").ContinueWith(t => MessageBox.Show(t.Result));

        }

        private void Login(object sender, RoutedEventArgs e)
        {
            EasyBayClient.GetToken("GurritoN", "Apz3cmcm").ContinueWith(t => { token = t.Result; client = new EasyBayClient(token); MessageBox.Show(token); });
        }

        private void Get(object sender, RoutedEventArgs e)
        {
            client.GetUser("GurritoN").ContinueWith(t => MessageBox.Show(t.Result));
        }
    }
}
