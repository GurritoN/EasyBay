using EasyBay.Messaging;
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
    /// Interaction logic for CreateLotWindow.xaml
    /// </summary>
    public partial class CreateLotWindow : Window
    {
        EasyBayClient client;
        public CreateLotWindow(EasyBayClient client)
        {
            InitializeComponent();
            this.client = client;
        }

        private async void Create(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text))
            {
                Error.Text = "Name is empty";
                return;
            }
            decimal sPrice, bPrice;
            try
            {
                sPrice = decimal.Parse(SPrice.Text);
                bPrice = decimal.Parse(BPrice.Text);
                if (sPrice >= bPrice)
                    throw new Exception();
            }
            catch
            {
                Error.Text = "Price is invalid";
                return;
            }
            if (!Finish.SelectedDate.HasValue)
            {
                Error.Text = "Choose trade finish time";
                return;
            }
            string[] tags = Tags.Text.Split(',');
            CreateLotRequest request = new CreateLotRequest();
            request.Name = Name.Text;
            request.Description = Desc.Text;
            request.TradeFinishTime = Finish.SelectedDate.Value;
            request.StartingPrice = sPrice;
            request.BuyOutPrice = bPrice;
            request.Tags = tags.ToList();
            bool result = await client.CreateLot(request);
            if (result)
            {
                Close();
            }
            else
            {
                Error.Text = "Error has been occured";
                return;
            }
        }
    }
}
