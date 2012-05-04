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
using System.IO;
using System.Text;

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
            //NavigationService.Navigate(new Uri("/Photo.xaml", UriKind.Relative)); return;
        }

        private void listenerTap(object sender, GestureEventArgs e)
        {

        }

        void onFinish(Model.File file)
        {
            Node node = new Node();
            node.Title = "Sent from my WinPhone7";
            node.Type = "catchsmile";
            node.File = file;

            RESTService service = new RESTService(AppResources.RESTServiceUri);
            service.CreateNode(node, onFinish2, onError);
        }

        void onError(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        void onFinish2(Model.Node node)
        {
            MessageBox.Show(node.Nid.ToString() + " - " + node.File.Fid.ToString());
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Photo.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            App.ViewModel.LoadData();

            // Call the base method.
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

            // Save changes to the database.
            App.ViewModel.SaveChangesToDB();
        }
    }
}