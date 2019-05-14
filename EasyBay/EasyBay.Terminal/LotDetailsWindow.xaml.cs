using EasyBay.Terminal.API;
using EasyBay.Terminal.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for LotDetailsWindow.xaml
    /// </summary>
    public partial class LotDetailsWindow : Window
    {
        EasyBayClient client;
        Lot lot;
        int userID;
        public LotDetailsWindow(EasyBayClient client, int lotID, int userID)
        {
            InitializeComponent();
            this.client = client;
            this.userID = userID;
            var task = client.GetLot(lotID).ContinueWith(t => InitValues(t.Result));
            //var uriSource = new Uri(@"../../img/default.jpg", UriKind.Relative);
            //File.Create(@"../../img/defaulat.jpg");
            //Img.Source = new BitmapImage(uriSource);
        }

        void InitValues(Lot lot)
        {
            this.lot = lot;
            Dispatcher.Invoke(() =>
            {
                LotName.Text = lot.Name;
                Desc.Text = lot.Description;
                Current.Text = lot.CurrentPrice.ToString();
                Buyout.Text = lot.BuyOutPrice.ToString();
                Finish.Text = lot.TradeFinishTime.ToString();
                NewPrice.Text = lot.CurrentPrice.ToString();
                if (lot.OwnerId == userID)
                    DeleteBtn.Visibility = Visibility.Visible;
            });
        }

        private async void Raise(object sender, RoutedEventArgs e)
        {
            if (await client.RaisePrice(lot.Id, decimal.Parse(NewPrice.Text)))
                Current.Text = NewPrice.Text;
        }

        private async void BuyoutBtn(object sender, RoutedEventArgs e)
        {
            if (await client.BuyOut(lot.Id))
                Close();
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if (await client.DeleteLot(lot.Id))
                Close();
        }
    }
}
