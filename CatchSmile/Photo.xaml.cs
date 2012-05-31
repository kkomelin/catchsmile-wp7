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
using WindowsPhoneRecipes;

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
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
            {
                this.Camera = new PhotoCamera(CameraType.Primary);
            }
            else
            {
                MessageBox.Show("Cannot find a camera on this device");
                return;
            }

            this.cameraViewBrush.SetSource(this.Camera);

            this.Camera.CaptureImageAvailable += new EventHandler<ContentReadyEventArgs>(Camera_CaptureImageAvailable);
            this.Camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(Camera_CaptureThumbnailAvailable);

            this.CorrectViewFinderOrientation(this.Orientation);

            this.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(PhoneApplicationPage_OrientationChanged);
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

        void onFileCreatedRemotely(Model.File file)
        {
            App.ViewModel.AddFile(file);

            Node node = new Node();
            node.Title = file.FileName;
            node.Type = AppResources.DrupalContentType;
            node.File = file;

            RESTService service = new RESTService(AppResources.RESTServiceUri);
            service.CreateNode(node, onNodeCreatedRemotely, onError);
        }

        void onError(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        void onNodeCreatedRemotely(Model.Node node)
        {
            App.ViewModel.AddNode(node);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Save file.
                string filename = string.Format("{0:yyyyMMdd-HHmmss}.jpg", DateTime.Now);

                int reducingRate = Int32.Parse(AppResources.ReducingRate);
                int photoQuality = Int32.Parse(AppResources.PhotoQuality);

                FileStorage.SaveToIsolatedStorage(e.ImageStream, filename, reducingRate, photoQuality);

                Model.File file = new Model.File();
                file.FileName = filename;
                file.FileSize = FileStorage.FileSize(filename);
                file.Uid = 0;

                RESTService service = new RESTService(AppResources.RESTServiceUri);
                service.CreateFile(file, onFileCreatedRemotely, onError);
            });
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.Camera.CaptureImage();
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            this.CorrectViewFinderOrientation(e.Orientation);
            base.OnOrientationChanged(e);
        }

        private void CorrectViewFinderOrientation(PageOrientation orientation)
        {

            if (this.Camera == null)
            {
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                System.Diagnostics.Debug.WriteLine("Switching to {0}", orientation);
                switch (orientation)
                {
                    case PageOrientation.PortraitUp:
                        this.cameraViewBrush.Transform =
                            new CompositeTransform { Rotation = 90, TranslateX = 480 };
                        this.lastShot.RenderTransform = new CompositeTransform { Rotation = 90, TranslateX = 480 };
                        break;
                    case PageOrientation.LandscapeLeft:
                        this.cameraViewBrush.Transform = null;
                        this.lastShot.RenderTransform = null;
                        break;
                    case PageOrientation.LandscapeRight:
                        if (Microsoft.Phone.Shell.SystemTray.IsVisible)
                        {
                            this.cameraViewBrush.Transform =
                                new CompositeTransform { Rotation = 180, TranslateX = 728, TranslateY = 480 };
                            this.lastShot.RenderTransform =
                            new CompositeTransform { Rotation = 180, TranslateX = 728, TranslateY = 480 };
                        }
                        else
                        {
                            this.cameraViewBrush.Transform =
                                new CompositeTransform { Rotation = 180, TranslateX = 800, TranslateY = 480 };
                            this.lastShot.RenderTransform =
                            new CompositeTransform { Rotation = 180, TranslateX = 800, TranslateY = 480 };
                        }
                        break;
                }
            });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            // Due to bug, BackKey doesn't send navigation events so we handle this.
            if (NavigationService.CanGoBack)
            {
                e.Cancel = true;
                NavigationService.GoBack();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // If we are in recursive back - DO NOT DO ANY WORK ON PAGE.
            // Developers - make sure you have no specific logic that you need to take care of here.
            if (NonLinearNavigationService.Instance.IsRecursiveBackNavigation == true)
            {
                return;
            }

            this.ApplicationBar.IsVisible = true;
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);

            // On navigating away from the page, hide the appbar so we dont see it if we are in recursive back.
            this.ApplicationBar.IsVisible = false;

            if (this.Camera == null)
            {
                return;
            }

            // Dispose of the camera to minimize power consumption and to expedite shutdown.
            this.Camera.Dispose();
        }
    }
}