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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            // due to bug, BackKey doesnt send navigation events so we handle this myself
            if (NavigationService.CanGoBack)
            {
                e.Cancel = true;
                NavigationService.GoBack();
            }
        }
    }
}