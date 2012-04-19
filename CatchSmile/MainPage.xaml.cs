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
using CatchSmile.Models;

namespace CatchSmile
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Camera object.
        /// </summary>
        private PhotoCamera Camera { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
          /*  // Check supported camera types.
            if (PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing))
            {
                this.Camera = new PhotoCamera(CameraType.FrontFacing);
            }
            else if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
            {
                this.Camera = new PhotoCamera(CameraType.Primary);
            }
            else // Display error message if no one type is supported.
            {
                MessageBox.Show("Cannot find a camera on this device");
                return;
            }

            this.cameraViewBrush.SetSource(this.Camera);

            this.Camera.CaptureImageAvailable += new EventHandler<ContentReadyEventArgs>(Camera_CaptureImageAvailable);
            this.Camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(Camera_CaptureThumbnailAvailable);*/
        }

        void Camera_CaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {

         /*   Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                BitmapImage image = new BitmapImage();

                image.SetSource(e.ImageStream);

                this.lastShot.Source = image;

                this.lastShotFrame.Visibility = System.Windows.Visibility.Visible;

                this.cameraView.Visibility = System.Windows.Visibility.Collapsed;
            });
            */
        }

        private void Camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            /*
            Deployment.Current.Dispatcher.BeginInvoke(() =>
             {

                 MediaLibrary ml = new MediaLibrary();

                 ml.SavePictureToCameraRoll(

                      string.Format("{0:yyyyMMdd-HHmmss}.jpg", DateTime.Now), e.ImageStream);

             });
            */
        }

        /// <summary>
        /// Process Tap event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listenerTap(object sender, GestureEventArgs e)
        {
            //this.Camera.CaptureImage();
        }

        void onFinish(Node node)
        {
            ObservableCollection<Node> oc = new ObservableCollection<Node>();
            oc.Add(node);


            listBox.ItemsSource = oc;
        }

        void onError(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            RESTService service = new RESTService(AppResources.RESTServiceUri);
            service.GetNode(20, onFinish, onError);
        }
    }
}