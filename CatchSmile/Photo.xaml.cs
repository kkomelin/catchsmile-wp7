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
using System.Text;
using CatchSmile.Services;
using CatchSmile.Resources;
using CatchSmile.Model;

namespace CatchSmile
{
    public partial class Photo : PhoneApplicationPage
    {

        /// <summary>
        /// Camera object.
        /// </summary>
        private PhotoCamera Camera { get; set; }

        public Photo()
        {
            InitializeComponent();
        }

        private void PhotoPage_Loaded(object sender, RoutedEventArgs e)
        {
              // Check supported camera types.
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
              this.Camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(Camera_CaptureThumbnailAvailable);
        }

        void Camera_CaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {

               Deployment.Current.Dispatcher.BeginInvoke(() =>
               {
                   BitmapImage image = new BitmapImage();

                   image.SetSource(e.ImageStream);

                   this.lastShot.Source = image;

                   this.lastShotFrame.Visibility = System.Windows.Visibility.Visible;

                   this.cameraView.Visibility = System.Windows.Visibility.Collapsed;
               });
         
        }

        void onFinish(Model.File file)
        {
            App.ViewModel.AddFile(file);

            Node node = new Node();
            node.Title = file.FileName;
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
            //MessageBox.Show("Hooray! The image has been posted to the site successfully!");

            App.ViewModel.AddNode(node);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
             {
                 string filename = string.Format("{0:yyyyMMdd-HHmmss}.jpg", DateTime.Now);

                 WriteableBitmap wb = FileStorage.SaveToIsolatedStorage(e.ImageStream, filename);

                 byte[] imageContent = FileStorage.ReadBytesFromIsolatedStorage(filename);

                 Model.File file = new Model.File();
                 file.FileName = filename;
                 file.FileSize = imageContent.Length;
                 file.FileContent = Convert.ToBase64String(imageContent);
                 file.Uid = 0;
                 

                 RESTService service = new RESTService(AppResources.RESTServiceUri);
                 service.CreateFile(file, onFinish, onError);

             });
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.Camera.CaptureImage();
        }
    }
}