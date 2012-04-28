using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using CatchSmile.Services;
using CatchSmile.Resources;
using System.Collections.ObjectModel;
using CatchSmile.Model;

namespace CatchSmile
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
      
        }

        private void listenerTap(object sender, GestureEventArgs e)
        {

        }

        void onFinish(Node node)
        {
            App.ViewModel.AddNode(node);

            listBox.ItemsSource = App.ViewModel.Nodes;
        }

        void onError(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            /*RESTService service = new RESTService(AppResources.RESTServiceUri);
            service.GetNode(20, onFinish, onError);*/

            //NavigationService.Navigate(new Uri("/Photo.xaml", UriKind.Relative));

            Node node = new Node();
            node.Title = "Sent from my WinPhone7";
            node.Type = "catchsmile";

            RESTService service = new RESTService(AppResources.RESTServiceUri);
            service.CreateNode(node, onFinish, onError);
        }
    }
}